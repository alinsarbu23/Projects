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
        var expectedId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var domain = new DomainCountry
        {
            Id = expectedId,
            Name = "Romania"
        };

        var dao = _mapper.Map<DaoCountry>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(expectedId);
        dao.Name.Should().Be("Romania");
    }

    [Fact]
    public void DaoCountry_To_DomainCountry_ShouldMapCorrectly()
    {
        var expectedId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var dao = new DaoCountry
        {
            Id = expectedId,
            Name = "Romania"
        };

        var domain = _mapper.Map<DomainCountry>(dao);

        domain.Should().NotBeNull();
        domain.Id.Should().Be(expectedId);
        domain.Name.Should().Be("Romania");
    }

    [Fact]
    public void CompanyClient_ShouldHoldData()
    {
        var dao = new CompanyClient
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            ClientId = Guid.Parse("33333333-3333-3333-3333-333333333334"),
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
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            ClientId = Guid.Parse("44444444-4444-4444-4444-444444444445"),
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
        var expectedId = Guid.Parse("55555555-5555-5555-5555-555555555555");
        var domain = new DomainClient
        {
            Id = expectedId,
            ClientTypeId = 1,
            Name = "Client",
            Cnp = "123",
            Email = "a@a.ro"
        };

        var dao = _mapper.Map<DaoClient>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(expectedId);
        dao.Name.Should().Be("Client");
        dao.Cnp.Should().Be("123");
        dao.Email.Should().Be("a@a.ro");
    }

    [Fact]
    public void DaoClient_To_DomainClient_ShouldMapCorrectly()
    {
        var expectedId = Guid.Parse("66666666-6666-6666-6666-666666666666");
        var dao = new DaoClient
        {
            Id = expectedId,
            ClientTypeId = 2,
            Name = "Company",
            Cui = "RO123"
        };

        var domain = _mapper.Map<DomainClient>(dao);

        domain.Should().NotBeNull();
        domain.Id.Should().Be(expectedId);
        domain.Name.Should().Be("Company");
        domain.Cui.Should().Be("RO123");
    }

    [Fact]
    public void DomainBuilding_To_DaoBuilding_ShouldMapCorrectly()
    {
        var expectedId = Guid.Parse("77777777-7777-7777-7777-777777777777");
        var domain = new DomainBuilding
        {
            Id = expectedId,
            Street = "Street",
            StreetNumber = "1",
            SurfaceArea = 10,
            InsuredValue = 100
        };

        var dao = _mapper.Map<DaoBuilding>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(expectedId);
        dao.Street.Should().Be("Street");
        dao.StreetNumber.Should().Be("1");
        dao.SurfaceArea.Should().Be(10);
        dao.InsuredValue.Should().Be(100);
    }

    [Fact]
    public void DomainClientIdentifier_To_DaoClientIdentifier_ShouldMapCorrectly()
    {
        var expectedId = Guid.Parse("88888888-8888-8888-8888-888888888888");
        var expectedClientId = Guid.Parse("88888888-8888-8888-8888-888888888889");
        var expectedChangedAt = DateTime.Parse("2020-01-01T00:00:00Z").ToUniversalTime();

        var domain = new DomainClientIdentifier
        {
            Id = expectedId,
            ClientId = expectedClientId,
            OldIdentifier = "OLD",
            NewIdentifier = "NEW",
            IdentifierType = "CNP",
            ChangedAt = expectedChangedAt
        };

        var dao = _mapper.Map<DaoClientIdentifier>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(expectedId);
        dao.ClientId.Should().Be(expectedClientId);
        dao.OldIdentifier.Should().Be("OLD");
        dao.NewIdentifier.Should().Be("NEW");
        dao.IdentifierType.Should().Be("CNP");
        dao.ChangedAt.Should().Be(expectedChangedAt);
    }

    [Fact]
    public void DomainFeeConfiguration_To_DaoFeeConfiguration_ShouldMapCorrectly()
    {
        var expectedId = Guid.Parse("99999999-9999-9999-9999-999999999999");
        var domain = new Insurance.Domain.Models.FeeConfiguration
        {
            Id = expectedId,
            Name = "Admin Fee",
            Type = Insurance.Domain.Enums.FeeType.AdminFee,
            Percentage = 5,
            IsActive = true
        };

        var dao = _mapper.Map<Insurance.Infrastructure.DAOs.FeeConfiguration>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(expectedId);
        dao.Name.Should().Be("Admin Fee");
        dao.Type.Should().Be("AdminFee");
        dao.Percentage.Should().Be(5);
        dao.IsActive.Should().BeTrue();
    }

    [Fact]
    public void DomainRiskFactorConfiguration_To_DaoRiskFactorConfiguration_ShouldMapCorrectly()
    {
        var expectedId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var domain = new Insurance.Domain.Models.RiskFactorConfiguration
        {
            Id = expectedId,
            Level = Insurance.Domain.Enums.RiskLevel.City,
            AdjustmentPercentage = 7.5m,
            IsActive = true
        };

        var dao = _mapper.Map<Insurance.Infrastructure.DAOs.RiskFactorConfiguration>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(expectedId);
        dao.Level.Should().Be("City");
        dao.AdjustmentPercentage.Should().Be(7.5m);
        dao.IsActive.Should().BeTrue();
    }

    [Fact]
    public void DomainCurrency_To_DaoCurrency_ShouldMapCorrectly()
    {
        var expectedId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var domain = new Insurance.Domain.Models.Currency
        {
            Id = expectedId,
            Code = "EUR",
            Name = "Euro",
            ExchangeRateToBase = 4.9m,
            IsActive = true
        };

        var dao = _mapper.Map<Insurance.Infrastructure.DAOs.Currency>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(expectedId);
        dao.Code.Should().Be("EUR");
        dao.Name.Should().Be("Euro");
        dao.ExchangeRateToBase.Should().Be(4.9m);
        dao.IsActive.Should().BeTrue();
    }

    [Fact]
    public void DomainBroker_To_DaoBroker_ShouldMapCorrectly()
    {
        var expectedId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
        var domain = new Insurance.Domain.Models.Broker
        {
            Id = expectedId,
            BrokerCode = "BR-01",
            Name = "Test Broker",
            Email = "broker@test.ro",
            Status = Insurance.Domain.Enums.BrokerStatus.Active
        };

        var dao = _mapper.Map<Insurance.Infrastructure.DAOs.Broker>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(expectedId);
        dao.BrokerCode.Should().Be("BR-01");
        dao.Name.Should().Be("Test Broker");
        dao.Email.Should().Be("broker@test.ro");
        dao.Status.Should().Be("Active");
    }

    [Fact]
    public void DomainPolicy_To_DaoPolicy_ShouldMapCorrectly()
    {
        var expectedId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
        var domain = new Insurance.Domain.Models.Policy
        {
            Id = expectedId,
            PolicyNumber = "POL-001",
            Status = Insurance.Domain.Enums.PolicyStatus.Active,
            BasePremium = 1000,
            FinalPremium = 1200
        };

        var dao = _mapper.Map<Insurance.Infrastructure.DAOs.Policy>(domain);

        dao.Should().NotBeNull();
        dao.Id.Should().Be(expectedId);
        dao.PolicyNumber.Should().Be("POL-001");
        dao.Status.Should().Be("Active");
        dao.BasePremium.Should().Be(1000);
        dao.FinalPremium.Should().Be(1200);
    }
}
