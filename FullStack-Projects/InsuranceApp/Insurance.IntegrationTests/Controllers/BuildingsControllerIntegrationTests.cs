using FluentAssertions;
using Insurance.Application.Dtos.Identity;
using Insurance.Application.DTOs.Buildings;
using Insurance.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.IntegrationTests.Controllers;

public class BuildingsIntegrationTests : BaseIntegrationTest
{
    public BuildingsIntegrationTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task CreateBuilding_ShouldPersist()
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
                brokerCode = "BR-BUILD",
                name = "Build Broker",
                email = "buildbroker@test.ro",
                phone = "0700000000",
                commissionPercentage = 2m
            });

        var brokerId = Guid.Parse(createBroker.Headers.Location!.Segments.Last());

        await Client.PostAsync($"/api/brokers/{brokerId}/activate", null);

        await Client.PostAsJsonAsync(
            "/api/auth/register",
            new { email = "buildbroker@test.ro", password = "Password1!" });

        var brokerLogin = await Client.PostAsJsonAsync(
            "/api/auth/login",
            new { email = "buildbroker@test.ro", password = "Password1!" });

        var brokerAuth = await brokerLogin.Content.ReadFromJsonAsync<AuthResponseDto>();
        Authenticate(brokerAuth!.Token);

        var createClientResponse = await Client.PostAsJsonAsync(
            "/api/clients",
            new
            {
                clientTypeId = 1,
                name = "Building Owner",
                cnp = "2990101223344",
                email = "owner@test.ro",
                phone = "0700000000",
                address = "Test Address"
            });

        createClientResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var clientId = Guid.Parse(createClientResponse.Headers.Location!.Segments.Last());

        var createBuildingResponse = await Client.PostAsJsonAsync(
            $"/api/buildings/client/{clientId}",
            new CreateBuildingDto
            {
                CityId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                BuildingTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Street = "Main Street",
                StreetNumber = "10",
                SurfaceArea = 75,
                InsuredValue = 150000,
                EarthquakeRisk = 1,
                FloodRisk = 1
            });

        createBuildingResponse.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
