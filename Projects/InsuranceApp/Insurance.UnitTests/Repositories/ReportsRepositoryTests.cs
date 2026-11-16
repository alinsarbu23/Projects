using Insurance.Application.DTOs.Reports;
using Insurance.Infrastructure;
using Insurance.Infrastructure.DAOs;
using Insurance.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Insurance.UnitTests.Repositories;

public class ReportsRepositoryTests
{
    private static InsuranceDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<InsuranceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InsuranceDbContext(options);
    }

    private static void AddMinimalData(InsuranceDbContext context, out Guid buildingTypeId)
    {
        var currency = new Currency
        {
            Id = Guid.NewGuid(),
            Code = "RON",
            Name = "Romanian Leu",
            ExchangeRateToBase = 1,
            IsActive = true
        };

        var broker = new Broker
        {
            Id = Guid.NewGuid(),
            BrokerCode = "BR-001",
            Name = "Test Broker",
            Email = "test@broker.ro",
            Phone = "0700000000",
            CommissionPercentage = 2.5m,
            Status = "Active"
        };

        buildingTypeId = Guid.NewGuid();

        var buildingType = new BuildingType
        {
            Id = buildingTypeId,
            Name = "Residential"
        };

        var country = new Country
        {
            Id = Guid.NewGuid(),
            Name = "Romania"
        };

        var county = new County
        {
            Id = Guid.NewGuid(),
            Name = "Cluj",
            Country = country,
            CountryId = country.Id
        };

        var city = new City
        {
            Id = Guid.NewGuid(),
            Name = "Cluj-Napoca",
            Code = "RO-CJ",
            County = county,
            CountyId = county.Id
        };

        var building = new Building
        {
            Id = Guid.NewGuid(),
            City = city,
            CityId = city.Id,
            BuildingType = buildingType,
            BuildingTypeId = buildingType.Id,
            Street = "Test",
            StreetNumber = "1",
            ConstructionYear = 2000,
            NumberOfFloors = 2,
            SurfaceArea = 100,
            InsuredValue = 200000,
            FloodRisk = true,
            EarthquakeRisk = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var policy = new Policy
        {
            Id = Guid.NewGuid(),
            Broker = broker,
            BrokerId = broker.Id,
            Currency = currency,
            CurrencyId = currency.Id,
            Building = building,
            BuildingId = building.Id,
            Status = "Active",
            StartDate = new DateTime(2024, 1, 1),
            EndDate = new DateTime(2025, 1, 1),
            FinalPremium = 1000
        };

        context.AddRange(currency, broker, buildingType, country, county, city, building, policy);
        context.SaveChanges();
    }

    [Fact]
    public async Task Filter_By_From_Should_Work()
    {
        using var context = CreateDbContext();
        AddMinimalData(context, out _);

        var repo = new ReportsRepository(context);

        var dto = new GetPoliciesReportDto { From = new DateTime(2024, 1, 1) };

        var (items, totalCount) = await repo.GetPoliciesByBrokerAsync(dto, 1, 10);

        Assert.Equal(1, totalCount);
        Assert.Single(items);
    }

    [Fact]
    public async Task Filter_By_BuildingType_Should_Work()
    {
        using var context = CreateDbContext();
        AddMinimalData(context, out var buildingTypeId);

        var repo = new ReportsRepository(context);

        var dto = new GetPoliciesReportDto { BuildingTypeId = buildingTypeId };

        var (items, totalCount) = await repo.GetPoliciesByBrokerAsync(dto, 1, 10);

        Assert.Equal(1, totalCount);
        Assert.Single(items);
    }

    [Fact]
    public async Task Group_By_Country_Should_Work()
    {
        using var context = CreateDbContext();
        AddMinimalData(context, out _);

        var repo = new ReportsRepository(context);

        var (items, totalCount) = await repo.GetPoliciesByCountryAsync(new GetPoliciesReportDto(), 1, 10);

        Assert.Equal(1, totalCount);
        Assert.Single(items);
        Assert.Equal("Romania", items.First().CountryName);
    }

    [Fact]
    public async Task Group_By_County_Should_Work()
    {
        using var context = CreateDbContext();
        AddMinimalData(context, out _);

        var repo = new ReportsRepository(context);

        var (items, totalCount) = await repo.GetPoliciesByCountyAsync(new GetPoliciesReportDto(), 1, 10);

        Assert.Equal(1, totalCount);
        Assert.Single(items);
        Assert.Equal("Cluj", items.First().CountyName);
    }

    [Fact]
    public async Task Group_By_City_Should_Work()
    {
        using var context = CreateDbContext();
        AddMinimalData(context, out _);

        var repo = new ReportsRepository(context);

        var (items, totalCount) = await repo.GetPoliciesByCityAsync(new GetPoliciesReportDto(), 1, 10);

        Assert.Equal(1, totalCount);
        Assert.Single(items);
        Assert.Equal("Cluj-Napoca", items.First().CityName);
    }
}
