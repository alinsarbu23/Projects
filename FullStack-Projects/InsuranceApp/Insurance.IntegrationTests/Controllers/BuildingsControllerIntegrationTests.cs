using FluentAssertions;
using Insurance.Application.DTOs.Buildings;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.DTOs.Geography;
using Insurance.Domain.Enums;
using Insurance.IntegrationTests.Helpers;
using Insurance.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.IntegrationTests.Controllers.Buildings;

public class BuildingsIntegrationTests : BaseIntegrationTest
{
    public BuildingsIntegrationTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task CreateBuilding_ShouldPersist()
    {
        var token = await AuthHelper.LoginAsBroker(Client);
        Authenticate(token);

        var createClientResponse = await Client.PostAsJsonAsync(
            "/api/brokers/clients",
            new CreateClientDto
            {
                ClientType = ClientType.Individual,
                Name = "Building Owner",
                Cnp = "2990101223344", 
                Email = "owner@test.ro",
                Phone = "0700000000",
                Address = "Test Address"
            });

        createClientResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var clientLocation = createClientResponse.Headers.Location;
        clientLocation.Should().NotBeNull();

        var countyId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

        var cities = await Client.GetFromJsonAsync<List<GetCitiesByCountyDto>>(
            $"/api/brokers/counties/{countyId}/cities");

        cities.Should().NotBeNull();
        cities!.Should().NotBeEmpty();

        var cityId = cities[0].Id;

        var createBuildingResponse = await Client.PostAsJsonAsync(
            $"{clientLocation}/buildings",
            new CreateBuildingDto
            {
                CityId = cityId,
                BuildingTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Street = "Main Street",
                StreetNumber = "10",
                SurfaceArea = 75,
                InsuredValue = 150000
            });

        createBuildingResponse.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
