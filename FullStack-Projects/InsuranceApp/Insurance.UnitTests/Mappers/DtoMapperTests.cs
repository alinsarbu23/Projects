using AutoMapper;
using FluentAssertions;
using Insurance.Application.DTOs.Brokers;
using Insurance.Application.DTOs.Buildings;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.DTOs.Currencies;
using Insurance.Application.DTOs.Fees;
using Insurance.Application.DTOs.Geography;
using Insurance.Application.DTOs.Policies;
using Insurance.Application.DTOs.RiskFactors;
using Insurance.Domain.Models;
using Insurance.Domain.Models.Geography;
using Insurance.UnitTests.Helpers;
using Xunit;

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
            ClientTypeId = 1,
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
            FloodRisk = 1,
            EarthquakeRisk = 1
        };

        dto.ConstructionYear.Should().Be(1999);
        dto.NumberOfFloors.Should().Be(3);
        dto.FloodRisk.Should().Be(1);
        dto.EarthquakeRisk.Should().Be(1);
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

    [Fact]
    public void FeeConfiguration_To_GetFeeConfigurationByIdDto_ShouldMapCorrectly()
    {
        var fee = new Insurance.Domain.Models.FeeConfiguration
        {
            Id = Guid.NewGuid(),
            Name = "Admin Fee",
            Type = Insurance.Domain.Enums.FeeType.AdminFee,
            Percentage = 5,
            IsActive = true
        };

        var dto = _mapper.Map<Insurance.Application.DTOs.Fees.GetFeeConfigurationByIdDto>(fee);

        dto.Id.Should().Be(fee.Id);
        dto.Name.Should().Be("Admin Fee");
        dto.Percentage.Should().Be(5);
        dto.IsActive.Should().BeTrue();
    }

    [Fact]
    public void RiskFactorConfiguration_To_GetRiskFactorConfigurationByIdDto_ShouldMapCorrectly()
    {
        var risk = new Insurance.Domain.Models.RiskFactorConfiguration
        {
            Id = Guid.NewGuid(),
            Level = Insurance.Domain.Enums.RiskLevel.County,
            AdjustmentPercentage = 3,
            IsActive = true
        };

        var dto = _mapper.Map<Insurance.Application.DTOs.RiskFactors.GetRiskFactorConfigurationByIdDto>(risk);

        dto.Id.Should().Be(risk.Id);
        dto.Level.Should().Be("County");
        dto.AdjustmentPercentage.Should().Be(3);
        dto.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Currency_To_GetCurrencyByIdDto_ShouldMapCorrectly()
    {
        var currency = new Insurance.Domain.Models.Currency
        {
            Id = Guid.NewGuid(),
            Code = "RON",
            Name = "Romanian Leu",
            ExchangeRateToBase = 1,
            IsActive = true
        };

        var dto = _mapper.Map<Insurance.Application.DTOs.Currencies.GetCurrencyByIdDto>(currency);

        dto.Id.Should().Be(currency.Id);
        dto.Code.Should().Be("RON");
        dto.Name.Should().Be("Romanian Leu");
        dto.ExchangeRateToBase.Should().Be(1);
        dto.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Broker_To_GetBrokerByIdDto_ShouldMapCorrectly()
    {
        var broker = new Insurance.Domain.Models.Broker
        {
            Id = Guid.NewGuid(),
            BrokerCode = "BR-02",
            Name = "Broker Test",
            Email = "b@test.ro",
            Status = Insurance.Domain.Enums.BrokerStatus.Inactive
        };

        var dto = _mapper.Map<Insurance.Application.DTOs.Brokers.GetBrokerByIdDto>(broker);

        dto.Id.Should().Be(broker.Id);
        dto.BrokerCode.Should().Be("BR-02");
        dto.Name.Should().Be("Broker Test");
        dto.Status.Should().Be("Inactive");
    }

    [Fact]
    public void Policy_To_GetPolicyByIdDto_ShouldMapCorrectly()
    {
        var policy = new Insurance.Domain.Models.Policy
        {
            Id = Guid.NewGuid(),
            PolicyNumber = "POL-999",
            BasePremium = 500,
            FinalPremium = 650,
            Building = new Insurance.Domain.Models.Building
            {
                Street = "Main",
                StreetNumber = "10"
            }
        };

        var dto = _mapper.Map<Insurance.Application.DTOs.Policies.GetPolicyByIdDto>(policy);

        dto.Id.Should().Be(policy.Id);
        dto.PolicyNumber.Should().Be("POL-999");
        dto.BasePremium.Should().Be(500);
        dto.FinalPremium.Should().Be(650);
        dto.BuildingAddress.Should().Contain("Main");
    }

    [Fact]
    public void GetBrokersDto_ShouldHoldData()
    {
        var dto = new GetBrokersDto
        {
            Id = Guid.NewGuid(),
            BrokerCode = "BR-01",
            Name = "Broker",
            Status = "Active"
        };

        dto.Status.Should().Be("Active");
    }

    [Fact]
    public void GetCurrenciesDto_ShouldHoldData()
    {
        var dto = new GetCurrenciesDto
        {
            Id = Guid.NewGuid(),
            Code = "RON",
            IsActive = true
        };

        dto.Code.Should().Be("RON");
        dto.IsActive.Should().BeTrue();
    }

    [Fact]
    public void GetFeeConfigurationsDto_ShouldHoldData()
    {
        var dto = new GetFeeConfigurationsDto
        {
            Id = Guid.NewGuid(),
            Name = "Admin Fee",
            Type = "AdminFee"
        };

        dto.Type.Should().Be("AdminFee");
    }

    [Fact]
    public void GetPoliciesDto_ShouldHoldData()
    {
        var dto = new GetPoliciesDto
        {
            Id = Guid.NewGuid(),
            PolicyNumber = "POL-1",
            Status = "Active",
            FinalPremium = 1000
        };

        dto.FinalPremium.Should().Be(1000);
    }

    [Fact]
    public void PolicyReportDto_ShouldHoldAggregates()
    {
        var dto = new PolicyReportDto
        {
            PolicyCount = 5,
            TotalFinalPremium = 5000
        };

        dto.PolicyCount.Should().Be(5);
    }

    [Fact]
    public void GetRiskFactorConfigurationsDto_ShouldHoldLevel()
    {
        var dto = new GetRiskFactorConfigurationsDto
        {
            Level = "City"
        };

        dto.Level.Should().Be("City");
    }

    [Fact]
    public void PagedPoliciesDto_ShouldHoldPagingInfo()
    {
        var dto = new PagedPoliciesDto
        {
            Page = 2,
            PageSize = 10,
            TotalCount = 50
        };

        dto.Page.Should().Be(2);
        dto.TotalCount.Should().Be(50);
    }
}
