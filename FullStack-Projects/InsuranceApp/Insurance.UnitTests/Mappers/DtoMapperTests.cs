using Xunit;
using FluentAssertions;
using AutoMapper;
using Insurance.UnitTests.Helpers;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.DTOs.Buildings;
using Insurance.Application.DTOs.Geography;
using Insurance.Domain.Models;
using Insurance.Domain.Models.Geography;
using Insurance.Domain.Enums;

namespace Insurance.UnitTests.Mappers;

public class DtoMapperTests
{
    private readonly IMapper _mapper = MapperFactory.Create();

    [Fact]
    public void Client_To_GetClientsDto_ShouldMapBasicFields()
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            ClientType = ClientType.Individual,
            Name = "Client"
        };

        var dto = _mapper.Map<GetClientsDto>(client);

        dto.Id.Should().Be(client.Id);
        dto.Name.Should().Be(client.Name);
    }

    [Fact]
    public void Client_To_GetClientByIdDto_ShouldMapBasicFields()
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Name = "Client",
            Email = "a@a.ro",
            Phone = "0700",
            Address = "Addr"
        };

        var dto = _mapper.Map<GetClientByIdDto>(client);

        dto.Id.Should().Be(client.Id);
        dto.Name.Should().Be(client.Name);
        dto.Email.Should().Be(client.Email);
        dto.Phone.Should().Be(client.Phone);
        dto.Address.Should().Be(client.Address);
    }

    [Fact]
    public void CreateBuildingDto_ShouldHoldOptionalFields()
    {
        var dto = new CreateBuildingDto
        {
            CityId = Guid.NewGuid(),
            BuildingTypeId = Guid.NewGuid(),
            Street = "Street",
            StreetNumber = "1",
            ConstructionYear = 1999,
            NumberOfFloors = 3,
            SurfaceArea = 100,
            InsuredValue = 2000,
            FloodRisk = true,
            EarthquakeRisk = true
        };

        dto.ConstructionYear.Should().Be(1999);
        dto.NumberOfFloors.Should().Be(3);
        dto.FloodRisk.Should().BeTrue();
        dto.EarthquakeRisk.Should().BeTrue();
    }

    [Fact]
    public void GetBuildingByIdDto_ShouldHoldGeographyFields()
    {
        var dto = new GetBuildingByIdDto
        {
            Street = "Street",
            StreetNumber = "1",
            City = "Cluj",
            County = "Cluj",
            Country = "Romania",
            BuildingType = "Residential"
        };

        dto.City.Should().Be("Cluj");
        dto.County.Should().Be("Cluj");
        dto.Country.Should().Be("Romania");
        dto.BuildingType.Should().Be("Residential");
    }

    [Fact]
    public void GetClientsDto_ShouldHoldIdentifier()
    {
        var dto = new GetClientsDto
        {
            Id = Guid.NewGuid(),
            ClientType = "Individual",
            Name = "Ion",
            Identifier = "1234567890123"
        };

        dto.Identifier.Should().Be("1234567890123");
    }

    [Fact]
    public void Building_To_ClientBuildingSummaryDto_ShouldMapBasicFields()
    {
        var building = new Building
        {
            Id = Guid.NewGuid(),
            InsuredValue = 1000
        };

        var dto = _mapper.Map<ClientBuildingSummaryDto>(building);

        dto.Id.Should().Be(building.Id);
        dto.InsuredValue.Should().Be(building.InsuredValue);
    }

    [Fact]
    public void Building_To_GetBuildingByIdDto_ShouldMapBasicFields()
    {
        var building = new Building
        {
            Id = Guid.NewGuid(),
            Street = "Street",
            StreetNumber = "1",
            SurfaceArea = 50,
            InsuredValue = 1000,
            FloodRisk = true,
            EarthquakeRisk = false
        };

        var dto = _mapper.Map<GetBuildingByIdDto>(building);

        dto.Id.Should().Be(building.Id);
        dto.Street.Should().Be(building.Street);
        dto.StreetNumber.Should().Be(building.StreetNumber);
        dto.SurfaceArea.Should().Be(building.SurfaceArea);
        dto.InsuredValue.Should().Be(building.InsuredValue);
        dto.FloodRisk.Should().Be(building.FloodRisk);
        dto.EarthquakeRisk.Should().Be(building.EarthquakeRisk);
    }

    [Fact]
    public void GeographyMappings_ShouldMapCorrectly()
    {
        var country = new Country { Id = Guid.NewGuid(), Name = "RO" };
        var county = new County { Id = Guid.NewGuid(), CountryId = country.Id, Name = "Cluj" };
        var city = new City { Id = Guid.NewGuid(), CountyId = county.Id, Name = "Cluj-Napoca", Code = "CJ" };

        _mapper.Map<GetCountriesDto>(country).Name.Should().Be("RO");
        _mapper.Map<GetCountiesByCountryDto>(county).Name.Should().Be("Cluj");
        _mapper.Map<GetCitiesByCountyDto>(city).Code.Should().Be("CJ");
    }
}
