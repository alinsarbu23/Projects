using FluentAssertions;
using Insurance.Application.Dtos.Identity;
using Insurance.Application.DTOs.Brokers;
using Insurance.Application.DTOs.Buildings;
using Insurance.Application.DTOs.Clients;
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
        await LoginAsAdmin();

        var createBroker = await Client.PostAsJsonAsync("/api/brokers",
            new CreateBrokerDto
            {
                BrokerCode = "BR-DEACT",
                Name = "Deactivated Broker",
                Email = "deactivated@test.ro",
                Phone = "0700000000",
                CommissionPercentage = 1.5m
            });

        createBroker.StatusCode.Should().Be(HttpStatusCode.Created);

        var brokerId = Guid.Parse(createBroker.Headers.Location!.Segments.Last());

        var activateResponse = await Client.PostAsync($"/api/brokers/{brokerId}/activate", null);
        activateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var registerResponse = await Client.PostAsJsonAsync("/api/auth/register",
            new { email = "deactivated@test.ro", password = "Password1!" });

        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var brokerLogin = await Client.PostAsJsonAsync("/api/auth/login",
            new { email = "deactivated@test.ro", password = "Password1!" });

        var brokerAuth = await brokerLogin.Content.ReadFromJsonAsync<AuthResponseDto>();
        Authenticate(brokerAuth!.Token);

        var clientResponse = await Client.PostAsJsonAsync("/api/clients",
            new CreateClientDto
            {
                ClientTypeId = 1,
                Name = "Test Client",
                Cnp = "1990101223344",
                Email = "client@test.ro",
                Phone = "0700000000",
                Address = "Test Address"
            });

        clientResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var clientId = Guid.Parse(clientResponse.Headers.Location!.Segments.Last());

        var buildingResponse = await Client.PostAsJsonAsync(
            $"/api/buildings/client/{clientId}",
            new CreateBuildingDto
            {
                CityId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                BuildingTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Street = "Main",
                StreetNumber = "1",
                SurfaceArea = 80,
                InsuredValue = 200000,
                FloodRisk = 1,
                EarthquakeRisk = 1
            });

        buildingResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var buildingId = Guid.Parse(buildingResponse.Headers.Location!.Segments.Last());

        await LoginAsAdmin();

        var deactivateResponse = await Client.PostAsync($"/api/brokers/{brokerId}/deactivate", null);
        deactivateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        Authenticate(brokerAuth.Token);

        var response = await Client.PostAsJsonAsync("/api/policies",
            new CreatePolicyDto
            {
                BuildingId = buildingId,
                CurrencyId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(10),
                BasePremium = 500
            });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
