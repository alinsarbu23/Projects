using Insurance.Application.Dtos.Identity;
using Insurance.Application.DTOs.Identity;
using Insurance.Application.Interfaces.Services;
using Insurance.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Insurance.Infrastructure.Services;

public class AuthManager : IAuthManager
{
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _config;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthManager(
        UserManager<ApiUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _config = config;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        if (await _userManager.FindByEmailAsync(dto.Email) != null)
        {
            throw new ArgumentException("User with this email already exists.");
        }
            
        if (!await _roleManager.RoleExistsAsync(dto.Role))
        {
            throw new ArgumentException($"Role '{dto.Role}' does not exist.");
        }
            
        var user = new ApiUser
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            throw new ArgumentException(string.Join("; ", result.Errors.Select(e => e.Description)));
        }
            

        await _userManager.AddToRoleAsync(user, dto.Role);

        return new AuthResponseDto
        {
            Token = GenerateJwtToken(user, dto.Role)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email)?? throw new ArgumentException("Invalid credentials.");

        if (!await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            throw new ArgumentException("Invalid credentials.");
        }


        var role = (await _userManager.GetRolesAsync(user)).First();

        return new AuthResponseDto
        {
            Token = GenerateJwtToken(user, role)
        };
    }

    private string GenerateJwtToken(ApiUser user, string role)
    {
        var key = _config["Jwt:Key"]!;
        var issuer = _config["Jwt:Issuer"]!;
        var expiresMinutes = int.Parse(_config["Jwt:ExpiresMinutes"]!);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, role)
        };

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
