using Insurance.Application.Dtos.Identity;
using Insurance.Application.DTOs.Identity;

namespace Insurance.Application.Interfaces.Services;

public interface IAuthManager
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}
