using FluentAssertions;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Repositories;
using Insurance.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Insurance.UnitTests.Repositories;

public class CurrencyRepositoryTests
{
    private static InsuranceDbContext CreateDb()
    {
        return new InsuranceDbContext(
            new DbContextOptionsBuilder<InsuranceDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
    }

    [Fact]
    public async Task GetActiveAsync_ShouldReturnOnlyActiveCurrencies()
    {
        using var db = CreateDb();
        var repo = new CurrencyRepository(db, MapperFactory.Create());

        db.Currencies.AddRange(
            new Infrastructure.DAOs.Currency
            {
                Id = Guid.NewGuid(),
                Code = "RON",
                IsActive = true
            },
            new Infrastructure.DAOs.Currency
            {
                Id = Guid.NewGuid(),
                Code = "EUR",
                IsActive = false
            });

        await db.SaveChangesAsync();

        var result = await repo.GetActiveAsync();

        result.Should().HaveCount(1);
        result[0].Code.Should().Be("RON");
    }

    [Fact]
    public async Task GetByCodeAsync_ShouldReturnNull_WhenInactive()
    {
        using var db = CreateDb();
        var repo = new CurrencyRepository(db, MapperFactory.Create());

        db.Currencies.Add(new Infrastructure.DAOs.Currency
        {
            Id = Guid.NewGuid(),
            Code = "EUR",
            IsActive = false
        });

        await db.SaveChangesAsync();

        var result = await repo.GetByCodeAsync("EUR");

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldPersistCurrency()
    {
        using var db = CreateDb();
        var repo = new CurrencyRepository(db, MapperFactory.Create());

        var currency = new Insurance.Domain.Models.Currency
        {
            Id = Guid.NewGuid(),
            Code = "EUR",
            Name = "Euro",
            ExchangeRateToBase = 4.9m,
            IsActive = true
        };

        await repo.AddAsync(currency);
        await db.SaveChangesAsync();

        db.Currencies.Should().HaveCount(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyCurrency()
    {
        using var db = CreateDb();
        var repo = new CurrencyRepository(db, MapperFactory.Create());

        var dao = new Infrastructure.DAOs.Currency
        {
            Id = Guid.NewGuid(),
            Code = "EUR",
            Name = "Euro",
            ExchangeRateToBase = 4.9m,
            IsActive = true
        };

        db.Currencies.Add(dao);
        await db.SaveChangesAsync();

        var domain = MapperFactory.Create().Map<Insurance.Domain.Models.Currency>(dao);
        domain.ExchangeRateToBase = 5.1m;

        await repo.UpdateAsync(domain);
        await db.SaveChangesAsync();

        db.Currencies.First().ExchangeRateToBase.Should().Be(5.1m);
    }
    [Fact]
    public async Task GetAllAsync_ReturnsCurrenciesOrderedByCode()
    {
        using var db = CreateDb();
        var repo = new CurrencyRepository(db, MapperFactory.Create());

        db.Currencies.AddRange(
            new Infrastructure.DAOs.Currency { Id = Guid.NewGuid(), Code = "USD", Name = "US Dollar" },
            new Infrastructure.DAOs.Currency { Id = Guid.NewGuid(), Code = "EUR", Name = "Euro" }
        );

        await db.SaveChangesAsync();

        var result = await repo.GetAllAsync();

        result.First().Code.Should().Be("EUR");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCurrency_WhenExists()
    {
        using var db = CreateDb();
        var repo = new CurrencyRepository(db, MapperFactory.Create());

        var id = Guid.NewGuid();

        db.Currencies.Add(new Infrastructure.DAOs.Currency
        {
            Id = id,
            Code = "RON",
            Name = "Romanian Leu"
        });

        await db.SaveChangesAsync();

        var result = await repo.GetByIdAsync(id);

        result.Should().NotBeNull();
        result!.Code.Should().Be("RON");
    }
}
