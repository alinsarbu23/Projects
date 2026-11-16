using FluentAssertions;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Repositories;
using Insurance.Domain.Enums;
using Insurance.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Insurance.UnitTests.Repositories;

public class BrokerRepositoryTests
{
    private static InsuranceDbContext CreateDb()
    {
        return new InsuranceDbContext(
            new DbContextOptionsBuilder<InsuranceDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsBrokersOrderedByName()
    {
        using var db = CreateDb();
        var repo = new BrokerRepository(db, MapperFactory.Create());

        db.Brokers.AddRange(
            new Infrastructure.DAOs.Broker { Id = Guid.NewGuid(), Name = "Zed" },
            new Infrastructure.DAOs.Broker { Id = Guid.NewGuid(), Name = "Alice" });

        await db.SaveChangesAsync();

        var result = await repo.GetAllAsync();

        result.First().Name.Should().Be("Alice");
    }

    [Fact]
    public async Task IsActiveAsync_ShouldReturnTrue_WhenBrokerIsActive()
    {
        using var db = CreateDb();
        var repo = new BrokerRepository(db, MapperFactory.Create());

        var brokerId = Guid.NewGuid();

        db.Brokers.Add(new Infrastructure.DAOs.Broker
        {
            Id = brokerId,
            Status = BrokerStatus.Active.ToString()
        });

        await db.SaveChangesAsync();

        var isActive = await repo.IsActiveAsync(brokerId);

        isActive.Should().BeTrue();
    }
    [Fact]
    public async Task GetByCodeAsync_ReturnsBroker()
    {
        using var db = CreateDb();
        var repo = new BrokerRepository(db, MapperFactory.Create());

        db.Brokers.Add(new Infrastructure.DAOs.Broker
        {
            Id = Guid.NewGuid(),
            BrokerCode = "BR-01",
            Name = "Broker"
        });

        await db.SaveChangesAsync();

        var result = await repo.GetByCodeAsync("BR-01");

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_ReturnsBroker()
    {
        using var db = CreateDb();
        var repo = new BrokerRepository(db, MapperFactory.Create());

        db.Brokers.Add(new Infrastructure.DAOs.Broker
        {
            Id = Guid.NewGuid(),
            BrokerCode = "BR-02",
            Name = "Broker",
            Email = "test@broker.ro"
        });

        await db.SaveChangesAsync();

        var result = await repo.GetByEmailAsync("test@broker.ro");

        result.Should().NotBeNull();
    }
}
