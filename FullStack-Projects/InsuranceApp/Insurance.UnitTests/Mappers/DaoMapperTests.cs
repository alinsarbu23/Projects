using AutoMapper;
using FluentAssertions;

using Insurance.Infrastructure.DAOs;
using Insurance.UnitTests.Helpers;
using Xunit;
using DaoBuilding = Insurance.Infrastructure.DAOs.Building;
// DAO
using DaoClient = Insurance.Infrastructure.DAOs.Client;
using DaoClientIdentifier = Insurance.Infrastructure.DAOs.ClientIdentifier;
using DaoCountry = Insurance.Infrastructure.DAOs.Country;
using DomainBuilding = Insurance.Domain.Models.Building;
// Domain
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
}
