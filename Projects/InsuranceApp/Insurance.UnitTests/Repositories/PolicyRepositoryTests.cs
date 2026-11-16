using FluentAssertions;
using Insurance.Domain.Enums;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Repositories;
using Insurance.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Insurance.UnitTests.Repositories;

public class PolicyRepositoryTests
{
    private static InsuranceDbContext CreateDb()
    {
        return new InsuranceDbContext(
            new DbContextOptionsBuilder<InsuranceDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistPolicy()
    {
        using var db = CreateDb();
        var repo = new PolicyRepository(db, MapperFactory.Create());

        var policy = new Insurance.Domain.Models.Policy
        {
            Id = Guid.NewGuid(),
            BrokerId = Guid.NewGuid(),
            BuildingId = Guid.NewGuid(),
            CurrencyId = Guid.NewGuid(),
            Status = PolicyStatus.Draft,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(10),
            BasePremium = 100,
            FinalPremium = 120
        };

        await repo.AddAsync(policy);
        await db.SaveChangesAsync();

        db.Policies.Should().HaveCount(1);
    }

    [Fact]
    public async Task CountAsync_FiltersByBroker()
    {
        using var db = CreateDb();
        var repo = new PolicyRepository(db, MapperFactory.Create());

        var brokerId = Guid.NewGuid();
        var buildingId = Guid.NewGuid();

        db.Buildings.Add(new Infrastructure.DAOs.Building
        {
            Id = buildingId,
            ClientId = Guid.NewGuid(),
            CityId = Guid.NewGuid(),
            BuildingTypeId = Guid.NewGuid(),
            Street = "Street",
            StreetNumber = "1",
            SurfaceArea = 10,
            InsuredValue = 100
        });

        db.Policies.AddRange(
            new Infrastructure.DAOs.Policy
            {
                Id = Guid.NewGuid(),
                BrokerId = brokerId,
                BuildingId = buildingId,
                Status = PolicyStatus.Active.ToString(),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10)
            },
            new Infrastructure.DAOs.Policy
            {
                Id = Guid.NewGuid(),
                BrokerId = Guid.NewGuid(),
                BuildingId = buildingId,
                Status = PolicyStatus.Active.ToString(),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10)
            });

        await db.SaveChangesAsync();

        var count = await repo.CountAsync(
            null,
            brokerId,
            null,
            null,
            null);

        count.Should().Be(1);
    }

    [Fact]
    public async Task CountAsync_StatusExpired_FiltersCorrectly()
    {
        using var db = CreateDb();
        var repo = new PolicyRepository(db, MapperFactory.Create());

        var brokerId = Guid.NewGuid();
        var buildingId = Guid.NewGuid();

        db.Buildings.Add(new Infrastructure.DAOs.Building
        {
            Id = buildingId,
            ClientId = Guid.NewGuid(),
            CityId = Guid.NewGuid(),
            BuildingTypeId = Guid.NewGuid(),
            Street = "Street",
            StreetNumber = "1",
            SurfaceArea = 10,
            InsuredValue = 100
        });

        db.Policies.Add(new Infrastructure.DAOs.Policy
        {
            Id = Guid.NewGuid(),
            BrokerId = brokerId,
            BuildingId = buildingId,
            Status = PolicyStatus.Active.ToString(),
            StartDate = DateTime.UtcNow.AddDays(-20),
            EndDate = DateTime.UtcNow.AddDays(-1)
        });

        await db.SaveChangesAsync();

        var count = await repo.CountAsync(
            null,
            brokerId,
            PolicyStatus.Expired,
            null,
            null);

        count.Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesPolicyFields()
    {
        using var db = CreateDb();
        var repo = new PolicyRepository(db, MapperFactory.Create());

        var id = Guid.NewGuid();

        db.Policies.Add(new Infrastructure.DAOs.Policy
        {
            Id = id,
            BrokerId = Guid.NewGuid(),
            BuildingId = Guid.NewGuid(),
            Status = PolicyStatus.Draft.ToString()
        });

        await db.SaveChangesAsync();

        var policy = new Insurance.Domain.Models.Policy
        {
            Id = id,
            Status = PolicyStatus.Active,
            ActivatedAt = DateTime.UtcNow
        };

        await repo.UpdateAsync(policy);
        await db.SaveChangesAsync();

        var dao = db.Policies.First();

        dao.Status.Should().Be("Active");
        dao.ActivatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task GetPolicyReportByBrokerAsync_GroupsAndSumsCorrectly()
    {
        using var db = CreateDb();
        var repo = new PolicyRepository(db, MapperFactory.Create());

        var brokerId = Guid.NewGuid();

        db.Brokers.Add(new Infrastructure.DAOs.Broker
        {
            Id = brokerId,
            BrokerCode = "BR-01",
            Name = "Broker"
        });

        db.Policies.AddRange(
            new Infrastructure.DAOs.Policy
            {
                Id = Guid.NewGuid(),
                BrokerId = brokerId,
                Status = PolicyStatus.Active.ToString(),
                FinalPremium = 100,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10)
            },
            new Infrastructure.DAOs.Policy
            {
                Id = Guid.NewGuid(),
                BrokerId = brokerId,
                Status = PolicyStatus.Active.ToString(),
                FinalPremium = 200,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10)
            });

        await db.SaveChangesAsync();

        var result = await repo.GetPolicyReportByBrokerAsync(null, null);

        result.Should().HaveCount(1);
        result[0].PolicyCount.Should().Be(2);
        result[0].TotalFinalPremium.Should().Be(300);
    }

    [Fact]
    public async Task SearchAsync_ShouldApplyPaging()
    {
        using var db = CreateDb();
        var repo = new PolicyRepository(db, MapperFactory.Create());

        var brokerId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var buildingId = Guid.NewGuid();
        var currencyId = Guid.NewGuid();

        db.Currencies.Add(new Infrastructure.DAOs.Currency
        {
            Id = currencyId,
            Code = "CHF",
            Name = "Swiss Franc",
            ExchangeRateToBase = 5.1m,
            IsActive = true
        });

        db.Buildings.Add(new Infrastructure.DAOs.Building
        {
            Id = buildingId,
            ClientId = clientId,
            CityId = Guid.NewGuid(),
            BuildingTypeId = Guid.NewGuid(),
            Street = "Street",
            StreetNumber = "1",
            SurfaceArea = 10,
            InsuredValue = 100,
            FloodRisk = false,
            EarthquakeRisk = false
        });

        for (int i = 0; i < 5; i++)
        {
            db.Policies.Add(new Infrastructure.DAOs.Policy
            {
                Id = Guid.NewGuid(),
                BrokerId = brokerId,
                BuildingId = buildingId,
                CurrencyId = currencyId,
                BasePremium = 100,
                FinalPremium = 120,
                Status = PolicyStatus.Active.ToString(),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30)
            });
        }

        await db.SaveChangesAsync();

        var result = await repo.SearchAsync(
            clientId,
            brokerId,
            PolicyStatus.Active,
            null,
            null,
            page: 2,
            pageSize: 2);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task ExistsActivePolicyWithCurrencyAsync_ShouldReturnTrue_WhenExists()
    {
        using var db = CreateDb();
        var repo = new PolicyRepository(db, MapperFactory.Create());

        var currencyId = Guid.NewGuid();

        db.Policies.Add(new Infrastructure.DAOs.Policy
        {
            Id = Guid.NewGuid(),
            CurrencyId = currencyId,
            Status = PolicyStatus.Active.ToString(),
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(10)
        });

        await db.SaveChangesAsync();

        var exists = await repo.ExistsActivePolicyWithCurrencyAsync(currencyId);

        exists.Should().BeTrue();
    }
    [Fact]
    public async Task GetByIdAsync_ReturnsPolicyWithBuildingAndCurrency()
    {
        using var db = CreateDb();
        var repo = new PolicyRepository(db, MapperFactory.Create());

        var currencyId = Guid.NewGuid();
        var buildingId = Guid.NewGuid();
        var brokerId = Guid.NewGuid();

        db.Currencies.Add(new Infrastructure.DAOs.Currency
        {
            Id = currencyId,
            Code = "RON",
            Name = "Romanian Leu",
            ExchangeRateToBase = 1,
            IsActive = true
        });

        db.Buildings.Add(new Infrastructure.DAOs.Building
        {
            Id = buildingId,
            ClientId = Guid.NewGuid(),
            CityId = Guid.NewGuid(),
            BuildingTypeId = Guid.NewGuid(),
            Street = "Street",
            StreetNumber = "1",
            SurfaceArea = 10,
            InsuredValue = 100
        });

        var policyId = Guid.NewGuid();

        db.Policies.Add(new Infrastructure.DAOs.Policy
        {
            Id = policyId,
            BrokerId = brokerId,
            BuildingId = buildingId,
            CurrencyId = currencyId,
            Status = PolicyStatus.Active.ToString(),
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            FinalPremium = 100
        });

        await db.SaveChangesAsync();

        var policy = await repo.GetByIdAsync(policyId);

        policy.Should().NotBeNull();
        policy!.FinalPremium.Should().Be(100);
    }
}
