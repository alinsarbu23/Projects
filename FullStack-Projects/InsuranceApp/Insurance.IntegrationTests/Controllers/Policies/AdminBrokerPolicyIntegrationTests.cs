using FluentAssertions;
using Insurance.Application.Dtos.Identity;
using Insurance.Application.DTOs.Brokers;
using Insurance.Application.DTOs.Policies;
using Insurance.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.IntegrationTests.Controllers.Policies;

public class AdminBrokerPolicyIntegrationTests : BaseIntegrationTest
{
    public AdminBrokerPolicyIntegrationTests(CustomWebApplicationFactory factory)
        : base(factory) { }

    [Fact]
    public async Task DeactivatedBroker_ShouldNotBeAbleToCreatePolicy()
    {
        var adminLogin = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new { email = "admin@insurance.local", password = "Admin123!" });

        var adminAuth = await adminLogin.Content.ReadFromJsonAsync<AuthResponseDto>();
        Authenticate(adminAuth!.Token);

        var createBroker = await Client.PostAsJsonAsync(
            "/api/brokers",
            new CreateBrokerDto
            {
                BrokerCode = "BR-DEACT",
                Name = "Deactivated Broker",
                Email = "deactivated@test.ro",
                Phone = "0700000000",
                CommissionPercentage = 1.5m
            });

        var brokerId = Guid.Parse(createBroker.Headers.Location!.Segments.Last());

        await Client.PostAsync($"/api/brokers/{brokerId}/activate", null);
        await Client.PostAsync($"/api/brokers/{brokerId}/deactivate", null);

        await Client.PostAsJsonAsync(
            "/api/auth/register",
            new { email = "deactivated@test.ro", password = "Password1!" });

        var brokerLogin = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new { email = "deactivated@test.ro", password = "Password1!" });

        var brokerAuth = await brokerLogin.Content.ReadFromJsonAsync<AuthResponseDto>();
        Authenticate(brokerAuth!.Token);

        var response = await Client.PostAsJsonAsync(
            "/api/policies",
            new CreatePolicyDto
            {
                BuildingId = Guid.NewGuid(),
                CurrencyId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(10),
                BasePremium = 500
            });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
