using FluentAssertions;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Repositories;
using Insurance.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Insurance.UnitTests.Repositories;

public class RiskFactorConfigurationRepositoryTests
{
    private static InsuranceDbContext CreateDb()
    {
        return new InsuranceDbContext(
            new DbContextOptionsBuilder<InsuranceDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
    }

    [Fact]
    public async Task GetActiveForBuildingAsync_ShouldFilterByLevelAndReference()
    {
        using var db = CreateDb();
        var repo = new RiskFactorConfigurationRepository(db, MapperFactory.Create());

        var cityId = Guid.NewGuid();

        db.RiskFactorConfigurations.AddRange(
            new Infrastructure.DAOs.RiskFactorConfiguration
            {
                Id = Guid.NewGuid(),
                Level = "City",
                ReferenceId = cityId,
                AdjustmentPercentage = 5,
                IsActive = true
            },
            new Infrastructure.DAOs.RiskFactorConfiguration
            {
                Id = Guid.NewGuid(),
                Level = "City",
                ReferenceId = Guid.NewGuid(),
                AdjustmentPercentage = 10,
                IsActive = true
            });

        await db.SaveChangesAsync();

        var result = await repo.GetActiveForBuildingAsync(
            Guid.Empty, Guid.Empty, cityId, Guid.Empty);

        result.Should().HaveCount(1);
        result[0].AdjustmentPercentage.Should().Be(5);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistRiskFactor()
    {
        using var db = CreateDb();
        var repo = new RiskFactorConfigurationRepository(db, MapperFactory.Create());

        var risk = new Insurance.Domain.Models.RiskFactorConfiguration
        {
            Id = Guid.NewGuid(),
            Level = Insurance.Domain.Enums.RiskLevel.City,
            ReferenceId = Guid.NewGuid(),
            AdjustmentPercentage = 5,
            IsActive = true
        };

        await repo.AddAsync(risk);
        await db.SaveChangesAsync();

        db.RiskFactorConfigurations.Should().HaveCount(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingRiskFactor()
    {
        using var db = CreateDb();
        var repo = new RiskFactorConfigurationRepository(db, MapperFactory.Create());

        var dao = new Infrastructure.DAOs.RiskFactorConfiguration
        {
            Id = Guid.NewGuid(),
            Level = "City",
            ReferenceId = Guid.NewGuid(),
            AdjustmentPercentage = 5,
            IsActive = true
        };

        db.RiskFactorConfigurations.Add(dao);
        await db.SaveChangesAsync();

        var domain = MapperFactory.Create()
            .Map<Insurance.Domain.Models.RiskFactorConfiguration>(dao);

        domain.AdjustmentPercentage = 10;

        await repo.UpdateAsync(domain);
        await db.SaveChangesAsync();

        db.RiskFactorConfigurations.First().AdjustmentPercentage.Should().Be(10);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllRiskFactors()
    {
        using var db = CreateDb();
        var repo = new RiskFactorConfigurationRepository(db, MapperFactory.Create());

        db.RiskFactorConfigurations.AddRange(
            new Infrastructure.DAOs.RiskFactorConfiguration
            {
                Id = Guid.NewGuid(),
                Level = "City",
                ReferenceId = Guid.NewGuid(),
                AdjustmentPercentage = 5,
                IsActive = true
            },
            new Infrastructure.DAOs.RiskFactorConfiguration
            {
                Id = Guid.NewGuid(),
                Level = "County",
                ReferenceId = Guid.NewGuid(),
                AdjustmentPercentage = 3,
                IsActive = true
            });

        await db.SaveChangesAsync();

        var result = await repo.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsRiskFactor_WhenExists()
    {
        using var db = CreateDb();
        var repo = new RiskFactorConfigurationRepository(db, MapperFactory.Create());

        var id = Guid.NewGuid();

        db.RiskFactorConfigurations.Add(new Infrastructure.DAOs.RiskFactorConfiguration
        {
            Id = id,
            Level = "City",
            ReferenceId = Guid.NewGuid(),
            AdjustmentPercentage = 5,
            IsActive = true
        });

        await db.SaveChangesAsync();

        var result = await repo.GetByIdAsync(id);

        result.Should().NotBeNull();
        result!.AdjustmentPercentage.Should().Be(5);
    }
}
