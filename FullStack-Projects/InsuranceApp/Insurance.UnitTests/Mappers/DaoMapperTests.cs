using AutoMapper;
using FluentAssertions;
using Insurance.Infrastructure.DAOs;
using Insurance.UnitTests.Helpers;
using Xunit;

using DaoBuilding = Insurance.Infrastructure.DAOs.Building;
using DaoClient = Insurance.Infrastructure.DAOs.Client;
using DaoClientIdentifier = Insurance.Infrastructure.DAOs.ClientIdentifier;
using DaoCountry = Insurance.Infrastructure.DAOs.Country;
using DomainBuilding = Insurance.Domain.Models.Building;

using DomainClient = Insurance.Domain.Models.Client;
using DomainClientIdentifier = Insurance.Domain.Models.ClientIdentifier;
using DomainCountry = Insurance.Domain.Models.Geography.Country;

namespace Insurance.UnitTests.Mappers;

public class DaoMapperTests
{
    private readonly IMapper _mapper;
    public DaoMapperTests()
    {
        _mapper = MapperFactory.Create();
    }

    [Fact]
    public void DomainCountry_To_DaoCountry_ShouldMapCorrectly()
    {
        var domain = new DomainCountry
        {
            Id = Guid.NewGuid(),
            Name = "Romania"
        };

        var dao = _mapper.Map<DaoCountry>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(domain.Id);
        dao.Name.Should().Be(domain.Name);
    }

    [Fact]
    public void DaoCountry_To_DomainCountry_ShouldMapCorrectly()
    {
        var dao = new DaoCountry
        {
            Id = Guid.NewGuid(),
            Name = "Romania"
        };

        var domain = _mapper.Map<DomainCountry>(dao);

        domain.Should().NotBeNull();
        domain.Id.Should().Be(dao.Id);
        domain.Name.Should().Be(dao.Name);
    }

    [Fact]
    public void CompanyClient_ShouldHoldData()
    {
        var dao = new CompanyClient
        {
            Id = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
            CompanyName = "Test SRL",
            Cui = "RO123"
        };

        dao.CompanyName.Should().Be("Test SRL");
        dao.Cui.Should().Be("RO123");
    }

    [Fact]
    public void IndividualClient_ShouldHoldData()
    {
        var dao = new IndividualClient
        {
            Id = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
            FirstName = "Ion",
            LastName = "Popescu",
            Cnp = "1234567890123"
        };

        dao.FirstName.Should().Be("Ion");
        dao.LastName.Should().Be("Popescu");
        dao.Cnp.Should().Be("1234567890123");
    }

    [Fact]
    public void DomainClient_To_DaoClient_ShouldMapCorrectly()
    {
        var domain = new DomainClient
        {
            Id = Guid.NewGuid(),
            ClientTypeId = 1,
            Name = "Client",
            Cnp = "123",
            Email = "a@a.ro"
        };

        var dao = _mapper.Map<DaoClient>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(domain.Id);
        dao.Name.Should().Be(domain.Name);
        dao.Cnp.Should().Be(domain.Cnp);
        dao.Email.Should().Be(domain.Email);
    }

    [Fact]
    public void DaoClient_To_DomainClient_ShouldMapCorrectly()
    {
        var dao = new DaoClient
        {
            Id = Guid.NewGuid(),
            ClientTypeId = 2,
            Name = "Company",
            Cui = "RO123"
        };

        var domain = _mapper.Map<DomainClient>(dao);

        domain.Should().NotBeNull();
        domain.Id.Should().Be(dao.Id);
        domain.Name.Should().Be(dao.Name);
        domain.Cui.Should().Be(dao.Cui);
    }

    [Fact]
    public void DomainBuilding_To_DaoBuilding_ShouldMapCorrectly()
    {
        var domain = new DomainBuilding
        {
            Id = Guid.NewGuid(),
            Street = "Street",
            StreetNumber = "1",
            SurfaceArea = 10,
            InsuredValue = 100
        };

        var dao = _mapper.Map<DaoBuilding>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(domain.Id);
        dao.Street.Should().Be(domain.Street);
        dao.StreetNumber.Should().Be(domain.StreetNumber);
        dao.SurfaceArea.Should().Be(domain.SurfaceArea);
        dao.InsuredValue.Should().Be(domain.InsuredValue);
    }

    [Fact]
    public void DomainClientIdentifier_To_DaoClientIdentifier_ShouldMapCorrectly()
    {
        var domain = new DomainClientIdentifier
        {
            Id = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
            OldIdentifier = "OLD",
            NewIdentifier = "NEW",
            IdentifierType = "CNP",
            ChangedAt = DateTime.UtcNow
        };

        var dao = _mapper.Map<DaoClientIdentifier>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(domain.Id);
        dao.ClientId.Should().Be(domain.ClientId);
        dao.OldIdentifier.Should().Be(domain.OldIdentifier);
        dao.NewIdentifier.Should().Be(domain.NewIdentifier);
        dao.IdentifierType.Should().Be(domain.IdentifierType);
    }

    [Fact]
    public void DomainFeeConfiguration_To_DaoFeeConfiguration_ShouldMapCorrectly()
    {
        var domain = new Insurance.Domain.Models.FeeConfiguration
        {
            Id = Guid.NewGuid(),
            Name = "Admin Fee",
            Type = Insurance.Domain.Enums.FeeType.AdminFee,
            Percentage = 5,
            IsActive = true
        };

        var dao = _mapper.Map<Insurance.Infrastructure.DAOs.FeeConfiguration>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(domain.Id);
        dao.Name.Should().Be(domain.Name);
        dao.Type.Should().Be(domain.Type.ToString());
        dao.Percentage.Should().Be(domain.Percentage);
        dao.IsActive.Should().BeTrue();
    }

    [Fact]
    public void DomainRiskFactorConfiguration_To_DaoRiskFactorConfiguration_ShouldMapCorrectly()
    {
        var domain = new Insurance.Domain.Models.RiskFactorConfiguration
        {
            Id = Guid.NewGuid(),
            Level = Insurance.Domain.Enums.RiskLevel.City,
            AdjustmentPercentage = 7.5m,
            IsActive = true
        };

        var dao = _mapper.Map<Insurance.Infrastructure.DAOs.RiskFactorConfiguration>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(domain.Id);
        dao.Level.Should().Be("City");
        dao.AdjustmentPercentage.Should().Be(7.5m);
        dao.IsActive.Should().BeTrue();
    }

    [Fact]
    public void DomainCurrency_To_DaoCurrency_ShouldMapCorrectly()
    {
        var domain = new Insurance.Domain.Models.Currency
        {
            Id = Guid.NewGuid(),
            Code = "EUR",
            Name = "Euro",
            ExchangeRateToBase = 4.9m,
            IsActive = true
        };

        var dao = _mapper.Map<Insurance.Infrastructure.DAOs.Currency>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(domain.Id);
        dao.Code.Should().Be("EUR");
        dao.Name.Should().Be("Euro");
        dao.ExchangeRateToBase.Should().Be(4.9m);
        dao.IsActive.Should().BeTrue();
    }

    [Fact]
    public void DomainBroker_To_DaoBroker_ShouldMapCorrectly()
    {
        var domain = new Insurance.Domain.Models.Broker
        {
            Id = Guid.NewGuid(),
            BrokerCode = "BR-01",
            Name = "Test Broker",
            Email = "broker@test.ro",
            Status = Insurance.Domain.Enums.BrokerStatus.Active
        };

        var dao = _mapper.Map<Insurance.Infrastructure.DAOs.Broker>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(domain.Id);
        dao.BrokerCode.Should().Be("BR-01");
        dao.Name.Should().Be("Test Broker");
        dao.Email.Should().Be("broker@test.ro");
        dao.Status.Should().Be("Active");
    }

    [Fact]
    public void DomainPolicy_To_DaoPolicy_ShouldMapCorrectly()
    {
        var domain = new Insurance.Domain.Models.Policy
        {
            Id = Guid.NewGuid(),
            PolicyNumber = "POL-001",
            Status = Insurance.Domain.Enums.PolicyStatus.Active,
            BasePremium = 1000,
            FinalPremium = 1200
        };

        var dao = _mapper.Map<Insurance.Infrastructure.DAOs.Policy>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(domain.Id);
        dao.PolicyNumber.Should().Be("POL-001");
        dao.Status.Should().Be("Active");
        dao.BasePremium.Should().Be(1000);
        dao.FinalPremium.Should().Be(1200);
    }

}
