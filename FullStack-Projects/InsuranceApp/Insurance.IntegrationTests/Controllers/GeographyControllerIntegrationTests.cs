using FluentAssertions;
using Insurance.Application.DTOs.Geography;
using Insurance.IntegrationTests.Helpers;
using Insurance.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.IntegrationTests.Controllers.Geography;

public class GeographyIntegrationTests : BaseIntegrationTest
{
    public GeographyIntegrationTests(CustomWebApplicationFactory factory)
        : base(factory) { }

    [Fact]
    public async Task GetCountries_ShouldReturnRomania()
    {
        Authenticate(await AuthHelper.LoginAsBroker(Client));

        var response = await Client.GetAsync("/api/brokers/countries");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var countries = await response.Content
            .ReadFromJsonAsync<List<GetCountriesDto>>();

        countries.Should().Contain(c => c.Name == "Romania");
    }

    [Fact]
    public async Task GetCitiesByCounty_ShouldReturnCities()
    {
        Authenticate(await AuthHelper.LoginAsBroker(Client));

        var countyId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

        var response = await Client.GetAsync(
            $"/api/brokers/counties/{countyId}/cities");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var cities = await response.Content
            .ReadFromJsonAsync<List<GetCitiesByCountyDto>>();

        cities.Should().NotBeEmpty();
    }
}
