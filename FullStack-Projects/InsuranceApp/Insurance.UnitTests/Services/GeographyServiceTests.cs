using Xunit;
using Moq;
using FluentAssertions;
using Insurance.Application.Services;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Models.Geography;
using Insurance.UnitTests.Helpers;

namespace Insurance.UnitTests.Services;

public class GeographyServiceTests
{
    private GeographyService CreateService(
        Mock<IUnitOfWork> uowMock,
        Mock<IGeographyRepository> geographyRepoMock)
    {
        uowMock.Setup(x => x.Geography)
            .Returns(geographyRepoMock.Object);

        return new GeographyService(
            MapperFactory.Create(),
            uowMock.Object);
    }

    [Fact]
    public async Task GetCountriesAsync_ShouldReturnMappedCountries()
    {
        var geographyRepoMock = new Mock<IGeographyRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        geographyRepoMock.Setup(x => x.GetAllCountriesAsync())
            .ReturnsAsync(new List<Country>
            {
                new Country
                {
                    Id = Guid.NewGuid(),
                    Name = "Romania"
                }
            });

        var service = CreateService(uowMock, geographyRepoMock);

        var result = await service.GetCountriesAsync();

        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result[0].Name.Should().Be("Romania");
    }

    [Fact]
    public async Task GetCountiesByCountryAsync_ShouldReturnMappedCounties()
    {
        var geographyRepoMock = new Mock<IGeographyRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        geographyRepoMock
            .Setup(x => x.GetCountiesByCountryIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<County>
            {
                new County
                {
                    Id = Guid.NewGuid(),
                    CountryId = Guid.NewGuid(),
                    Name = "Cluj"
                }
            });

        var service = CreateService(uowMock, geographyRepoMock);

        var result = await service.GetCountiesByCountryAsync(Guid.NewGuid());

        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result[0].Name.Should().Be("Cluj");
    }

    [Fact]
    public async Task GetCitiesByCountyAsync_ShouldReturnMappedCities()
    {
        var geographyRepoMock = new Mock<IGeographyRepository>();
        var uowMock = new Mock<IUnitOfWork>();

        geographyRepoMock
            .Setup(x => x.GetCitiesByCountyIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<City>
            {
                new City
                {
                    Id = Guid.NewGuid(),
                    CountyId = Guid.NewGuid(),
                    Name = "Cluj-Napoca",
                    Code = "CJ"
                }
            });

        var service = CreateService(uowMock, geographyRepoMock);

        var result = await service.GetCitiesByCountyAsync(Guid.NewGuid());

        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result[0].Name.Should().Be("Cluj-Napoca");
        result[0].Code.Should().Be("CJ");
    }
}
