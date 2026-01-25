using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Insurance.Api.Controllers;
using Insurance.Application.DTOs.Geography;
using Insurance.Application.Interfaces.Services;

namespace Insurance.UnitTests.Controllers;

public class GeographyControllerTests
{
    [Fact]
    public async Task GetCountiesByCountry_ShouldReturnOkWithResult()
    {
        var service = new Mock<IGeographyService>();

        var countryId = Guid.NewGuid();
        var expected = new List<GetCountiesByCountryDto>
        {
            new GetCountiesByCountryDto { Id = Guid.NewGuid(), Name = "Cluj" }
        };

        service.Setup(x => x.GetCountiesByCountryAsync(countryId))
            .ReturnsAsync(expected);

        var controller = new GeographyController(service.Object);

        var actionResult = await controller.GetCountiesByCountry(countryId);

        var ok = actionResult.Result as OkObjectResult;
        ok.Should().NotBeNull();
        ok!.StatusCode.Should().Be(200);

        var payload = ok.Value as IReadOnlyList<GetCountiesByCountryDto>;
        payload.Should().NotBeNull();
        payload!.Count.Should().Be(1);
        payload[0].Name.Should().Be("Cluj");
    }

    [Fact]
    public async Task GetCountries_ShouldReturnOkWithCountries()
    {
        var service = new Mock<IGeographyService>();
        var controller = new GeographyController(service.Object);

        service.Setup(x => x.GetCountriesAsync())
            .ReturnsAsync(new List<GetCountriesDto>
            {
            new GetCountriesDto { Id = Guid.NewGuid(), Name = "Romania" }
            });

        var result = await controller.GetCountries();

        var ok = result.Result as OkObjectResult;
        ok.Should().NotBeNull();

        var payload = ok!.Value as IReadOnlyList<GetCountriesDto>;
        payload.Should().NotBeNull();
        payload![0].Name.Should().Be("Romania");
    }


    [Fact]
    public async Task GetCitiesByCounty_ShouldReturnOkWithResult()
    {
        var service = new Mock<IGeographyService>();

        var countyId = Guid.NewGuid();
        var expected = new List<GetCitiesByCountyDto>
        {
            new GetCitiesByCountyDto { Id = Guid.NewGuid(), Name = "Cluj-Napoca", Code = "CJ" }
        };

        service.Setup(x => x.GetCitiesByCountyAsync(countyId))
            .ReturnsAsync(expected);

        var controller = new GeographyController(service.Object);

        var actionResult = await controller.GetCitiesByCounty(countyId);

        var ok = actionResult.Result as OkObjectResult;
        ok.Should().NotBeNull();

        var payload = ok!.Value as IReadOnlyList<GetCitiesByCountyDto>;
        payload.Should().NotBeNull();
        payload![0].Code.Should().Be("CJ");
    }


}
