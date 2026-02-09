using FluentAssertions;
using Insurance.Application.Dtos.Identity;
using Insurance.Application.DTOs.Identity;
using Insurance.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.IntegrationTests.Controllers.Auth;

public class AuthIntegrationTests : BaseIntegrationTest
{
    public AuthIntegrationTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Register_And_Login_ShouldReturnJwt()
    {
        var registerResponse = await Client.PostAsJsonAsync(
            "/api/auth/register",
            new RegisterDto
            {
                Email = "auth_user@test.ro",
                Password = "Password1!"
            });

        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginResponse = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginDto
            {
                Email = "auth_user@test.ro",
                Password = "Password1!"
            });

        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var auth = await loginResponse.Content
            .ReadFromJsonAsync<AuthResponseDto>();

        auth.Should().NotBeNull();
        auth!.Token.Should().NotBeNullOrWhiteSpace();
    }
}
