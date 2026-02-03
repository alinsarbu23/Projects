using FluentAssertions;
using Insurance.Application.DTOs.Buildings;
using Insurance.Application.DTOs.Geography;
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
        Authenticate(await AuthHelper.Login(Client));

        var createClientResponse = await Client.PostAsJsonAsync(
            "/api/brokers/clients",
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

        var clientLocation = createClientResponse.Headers.Location!;
        var countyId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

        var cities = await Client.GetFromJsonAsync<List<GetCitiesByCountyDto>>(
            $"/api/brokers/counties/{countyId}/cities");

        var createBuildingResponse = await Client.PostAsJsonAsync(
            $"{clientLocation}/buildings",
            new CreateBuildingDto
            {
                CityId = cities![0].Id,
                BuildingTypeId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Street = "Main Street",
                StreetNumber = "10",
                SurfaceArea = 75,
                InsuredValue = 150000
            });

        createBuildingResponse.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
