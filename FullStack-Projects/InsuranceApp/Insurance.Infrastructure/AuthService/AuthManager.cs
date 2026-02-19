using Insurance.Application.Dtos.Identity;
using Insurance.Application.DTOs.Identity;
using Insurance.Application.Exceptions;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Domain.Enums;
using Insurance.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Insurance.Infrastructure.AuthService;

public class AuthManager : IAuthManager
{
    private readonly UserManager<ApiUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthManager> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AuthManager(
        UserManager<ApiUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config,
        IUnitOfWork unitOfWork,
        ILogger<AuthManager> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _config = config;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        _logger.LogInformation("Register attempt for {Email}", dto.Email);

        var broker = await _unitOfWork.Brokers.GetByEmailAsync(dto.Email);

        if (broker == null)
        {
            _logger.LogWarning("Register blocked. No broker found for {Email}", dto.Email);
            throw new NotFoundException("Broker account does not exist.");
        }

        if (broker.Status != BrokerStatus.Active)
        {
            _logger.LogWarning("Register blocked. Broker {BrokerId} is not active", broker.Id);
            throw new InvalidOperationException("Broker account is not active.");
        }

        if (await _userManager.FindByEmailAsync(dto.Email) != null)
        {
            _logger.LogWarning("Register blocked. Email {Email} already registered", dto.Email);
            throw new ArgumentException("User with this email already exists.");
        }

        if (!await _roleManager.RoleExistsAsync(Roles.Broker))
        {
            _logger.LogError("Critical configuration error. Role {Role} not found", Roles.Broker);
            throw new InvalidOperationException($"Role '{Roles.Broker}' is not configured.");
        }

        var user = new ApiUser
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            _logger.LogError("Register failed for {Email}. Errors: {Errors}",
                dto.Email,
                string.Join("; ", result.Errors.Select(e => e.Description)));

            throw new ArgumentException(
                string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        await _userManager.AddToRoleAsync(user, Roles.Broker);

        _logger.LogInformation("Broker {BrokerId} successfully registered", broker.Id);

        return new AuthResponseDto
        {
            Token = await GenerateJwtTokenAsync(user)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        _logger.LogInformation("Login attempt for {Email}", dto.Email);

        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            _logger.LogWarning("Login failed. User {Email} not found", dto.Email);
            throw new ArgumentException("Invalid credentials.");
        }

        if (!await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            _logger.LogWarning("Login failed. Invalid password for {Email}", dto.Email);
            throw new ArgumentException("Invalid credentials.");
        }

        _logger.LogInformation("User {Email} logged in successfully", dto.Email);

        return new AuthResponseDto
        {
            Token = await GenerateJwtTokenAsync(user)
        };
    }

    private async Task<string> GenerateJwtTokenAsync(ApiUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = _config["Jwt:Key"]!;
        var issuer = _config["Jwt:Issuer"]!;
        var expiresMinutes = int.Parse(_config["Jwt:ExpiresMinutes"]!);

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key));

        var token = new JwtSecurityToken(
            issuer,
            issuer,
            claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials:
                new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
