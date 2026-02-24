using FluentAssertions;
using Insurance.Domain.Enums;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Repositories;
using Insurance.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Insurance.UnitTests.Repositories;

public class FeeConfigurationRepositoryTests
{
    private static InsuranceDbContext CreateDb()
    {
        return new InsuranceDbContext(
            new DbContextOptionsBuilder<InsuranceDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
    }

    [Fact]
    public async Task GetFeeByIdAsync_ReturnsNull_WhenMissing()
    {
        using var db = CreateDb();
        var repo = new FeeConfigurationRepository(db, MapperFactory.Create());

        var result = await repo.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateFee()
    {
        using var db = CreateDb();
        var repo = new FeeConfigurationRepository(db, MapperFactory.Create());

        var dao = new Insurance.Infrastructure.DAOs.FeeConfiguration
        {
            Id = Guid.NewGuid(),
            Name = "Old",
            Type = "AdminFee",
            Percentage = 5,
            IsActive = true,
            EffectiveFrom = DateTime.UtcNow
        };

        db.FeeConfigurations.Add(dao);
        await db.SaveChangesAsync();

        var domain = MapperFactory.Create()
            .Map<Insurance.Domain.Models.FeeConfiguration>(dao);

        domain.Name = "New";

        await repo.UpdateAsync(domain);
        await db.SaveChangesAsync();

        db.FeeConfigurations.First().Name.Should().Be("New");
    }

    [Fact]
    public async Task AddAsync_ShouldPersistFee()
    {
        using var db = CreateDb();
        var repo = new FeeConfigurationRepository(db, MapperFactory.Create());

        await repo.AddAsync(new Insurance.Domain.Models.FeeConfiguration
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Type = Insurance.Domain.Enums.FeeType.AdminFee,
            Percentage = 5,
            IsActive = true
        });

        await db.SaveChangesAsync();

        db.FeeConfigurations.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllFees()
    {
        using var db = CreateDb();

        db.FeeConfigurations.Add(new Insurance.Infrastructure.DAOs.FeeConfiguration
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Type = "AdminFee",
            Percentage = 5,
            IsActive = true,
            EffectiveFrom = DateTime.UtcNow
        });

        await db.SaveChangesAsync();

        var repo = new FeeConfigurationRepository(db, MapperFactory.Create());

        var result = await repo.GetAllAsync();

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetActiveAsync_ShouldReturnOnlyActiveAndValidFees()
    {
        using var db = CreateDb();
        var repo = new FeeConfigurationRepository(db, MapperFactory.Create());

        db.FeeConfigurations.AddRange(
            new Infrastructure.DAOs.FeeConfiguration
            {
                Id = Guid.NewGuid(),
                Name = "Active Fee",
                Type = FeeType.AdminFee.ToString(),
                Percentage = 5,
                IsActive = true,
                EffectiveFrom = DateTime.UtcNow.AddDays(-1)
            },
            new Infrastructure.DAOs.FeeConfiguration
            {
                Id = Guid.NewGuid(),
                Name = "Inactive Fee",
                Type = FeeType.AdminFee.ToString(),
                Percentage = 10,
                IsActive = false,
                EffectiveFrom = DateTime.UtcNow.AddDays(-1)
            });

        await db.SaveChangesAsync();

        var result = await repo.GetActiveAsync(null, DateTime.UtcNow);

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Active Fee");
    }

    [Fact]
    public async Task GetActiveAsync_FiltersByType_WhenProvided()
    {
        using var db = CreateDb();
        var repo = new FeeConfigurationRepository(db, MapperFactory.Create());

        db.FeeConfigurations.AddRange(
            new Infrastructure.DAOs.FeeConfiguration
            {
                Id = Guid.NewGuid(),
                Name = "Admin Fee",
                Type = FeeType.AdminFee.ToString(),
                Percentage = 5,
                IsActive = true,
                EffectiveFrom = DateTime.UtcNow.AddDays(-1)
            },
            new Infrastructure.DAOs.FeeConfiguration
            {
                Id = Guid.NewGuid(),
                Name = "Risk Fee",
                Type = FeeType.RiskAdjustment.ToString(),
                Percentage = 10,
                IsActive = true,
                EffectiveFrom = DateTime.UtcNow.AddDays(-1)
            });

        await db.SaveChangesAsync();

        var result = await repo.GetActiveAsync(FeeType.AdminFee, DateTime.UtcNow);

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Admin Fee");
    }
}
