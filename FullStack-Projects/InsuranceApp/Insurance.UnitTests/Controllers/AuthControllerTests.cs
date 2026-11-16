using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Insurance.Api.Controllers;
using Insurance.Application.Interfaces.Services;
using Insurance.Application.DTOs.Identity;
using Insurance.Application.Dtos.Identity;

namespace Insurance.UnitTests.Controllers;

public class AuthControllerTests
{
    [Fact]
    public async Task Register_ShouldReturnOk_WithToken()
    {
        var authManager = new Mock<IAuthManager>();
        var controller = new AuthController(authManager.Object);

        authManager.Setup(x => x.RegisterAsync(It.IsAny<RegisterDto>()))
            .ReturnsAsync(new AuthResponseDto { Token = "JWT_TOKEN" });

        var result = await controller.Register(new RegisterDto
        {
            Email = "a@a.ro",
            Password = "Password1!",
        });

        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();

        var dto = ok!.Value as AuthResponseDto;
        dto.Should().NotBeNull();
        dto!.Token.Should().Be("JWT_TOKEN");
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WithToken()
    {
        var authManager = new Mock<IAuthManager>();
        var controller = new AuthController(authManager.Object);

        authManager.Setup(x => x.LoginAsync(It.IsAny<LoginDto>()))
            .ReturnsAsync(new AuthResponseDto { Token = "JWT_TOKEN" });

        var result = await controller.Login(new LoginDto
        {
            Email = "a@a.ro",
            Password = "Password1!"
        });

        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();
    }
}
