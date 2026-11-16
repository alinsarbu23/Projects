using FluentAssertions;
using Insurance.Application.Dtos.Identity;
using Insurance.Application.DTOs.Identity;
using Insurance.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.IntegrationTests.Auth;

public class AuthIntegrationTests : BaseIntegrationTest
{
    public AuthIntegrationTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Register_And_Login_ShouldReturnJwt()
    {
        var adminLogin = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new { email = "admin@insurance.local", password = "Admin123!" });

        var adminAuth = await adminLogin.Content.ReadFromJsonAsync<AuthResponseDto>();
        Authenticate(adminAuth!.Token);

        var createBroker = await Client.PostAsJsonAsync(
            "/api/brokers",
            new
            {
                brokerCode = "BR-AUTH",
                name = "Auth Broker",
                email = "auth@test.ro",
                phone = "0700000000",
                commissionPercentage = 2m
            });

        var brokerId = Guid.Parse(createBroker.Headers.Location!.Segments.Last());

        await Client.PostAsync($"/api/brokers/{brokerId}/activate", null);

        await Client.PostAsJsonAsync(
            "/api/auth/register",
            new RegisterDto
            {
                Email = "auth@test.ro",
                Password = "Password1!"
            });

        var loginResponse = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginDto
            {
                Email = "auth@test.ro",
                Password = "Password1!"
            });

        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var auth = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();

        auth.Should().NotBeNull();
        auth!.Token.Should().NotBeNullOrWhiteSpace();
    }
}
