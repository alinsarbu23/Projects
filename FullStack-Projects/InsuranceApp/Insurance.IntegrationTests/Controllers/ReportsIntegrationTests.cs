using FluentAssertions;
using Insurance.Application.DTOs.Reports;
using Insurance.Infrastructure;
using Insurance.Infrastructure.DAOs;
using Insurance.Infrastructure.Seed;
using Insurance.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Insurance.IntegrationTests.Controllers;

public class ReportsIntegrationTests : BaseIntegrationTest
{
    public ReportsIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    private async Task SeedReportsData()
    {
        using (var scope = Factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();

            await db.Database.EnsureDeletedAsync();
            await db.Database.MigrateAsync();

            await DatabaseSeeder.SeedAsync(scope.ServiceProvider);

            var city = db.Cities.First();

            var residential = db.BuildingTypes.First(x => x.Name == "Residential");
            var office = db.BuildingTypes.First(x => x.Name == "Office");

            var ron = db.Currencies.First(x => x.Code == "RON");

            var eur = new Currency
            {
                Id = Guid.NewGuid(),
                Code = "EUR",
                Name = "Euro",
                ExchangeRateToBase = 5,
                IsActive = true
            };

            db.Currencies.Add(eur);

            var client = new Client
            {
                Id = Guid.NewGuid(),
                ClientTypeId = db.ClientTypes.First().Id,
                Name = "Test Client"
            };

            db.Clients.Add(client);

            var broker1 = new Broker
            {
                Id = Guid.NewGuid(),
                BrokerCode = $"BR-{Guid.NewGuid()}",
                Name = "Broker 1",
                Status = "Active"
            };

            var broker2 = new Broker
            {
                Id = Guid.NewGuid(),
                BrokerCode = $"BR-{Guid.NewGuid()}",
                Name = "Broker 2",
                Status = "Active"
            };

            db.Brokers.AddRange(broker1, broker2);

            await db.SaveChangesAsync();

            var building1 = new Building
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,
                CityId = city.Id,
                BuildingTypeId = residential.Id,
                Street = "Street 1",
                StreetNumber = "1",
                SurfaceArea = 100,
                InsuredValue = 100000,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var building2 = new Building
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,
                CityId = city.Id,
                BuildingTypeId = office.Id,
                Street = "Street 2",
                StreetNumber = "2",
                SurfaceArea = 200,
                InsuredValue = 200000,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            db.Buildings.AddRange(building1, building2);

            await db.SaveChangesAsync();

            db.Policies.AddRange(
                new Policy
                {
                    Id = Guid.NewGuid(),
                    PolicyNumber = Guid.NewGuid().ToString(),
                    BrokerId = broker1.Id,
                    BuildingId = building1.Id,
                    CurrencyId = ron.Id,
                    Status = "Active",
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2024, 12, 31),
                    FinalPremium = 1000
                },
                new Policy
                {
                    Id = Guid.NewGuid(),
                    PolicyNumber = Guid.NewGuid().ToString(),
                    BrokerId = broker2.Id,
                    BuildingId = building2.Id,
                    CurrencyId = eur.Id,
                    Status = "Cancelled",
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2024, 12, 31),
                    FinalPremium = 200
                }
            );

            await db.SaveChangesAsync();
        }
    }

    [Fact]
    public async Task PoliciesByCity_ShouldAggregateCorrectly()
    {
        await SeedReportsData();
        await LoginAsAdmin();

        var response = await Client.GetAsync("/api/reports/policies-by-city?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResultDto<GetPoliciesByCityReportDto>>();

        result!.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task PoliciesByCounty_ShouldAggregateCorrectly()
    {
        await SeedReportsData();
        await LoginAsAdmin();

        var response = await Client.GetAsync("/api/reports/policies-by-county?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResultDto<GetPoliciesByCountyReportDto>>();

        result!.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task PoliciesByBroker_ShouldAggregateCorrectly()
    {
        await SeedReportsData();
        await LoginAsAdmin();

        var response = await Client.GetAsync("/api/reports/policies-by-broker?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResultDto<GetPoliciesByBrokerReportDto>>();

        result!.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task PoliciesByCity_ShouldFilterByCurrency()
    {
        await SeedReportsData();
        await LoginAsAdmin();

        var response = await Client.GetAsync("/api/reports/policies-by-city?currencyCode=RON&page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResultDto<GetPoliciesByCityReportDto>>();

        result!.Items.Should().HaveCount(1);
        result.Items[0].CurrencyCode.Should().Be("RON");
    }

    [Fact]
    public async Task PoliciesByCity_ShouldFilterByBuildingType()
    {
        await SeedReportsData();
        await LoginAsAdmin();

        Guid buildingTypeId;

        using (var scope = Factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
            buildingTypeId = db.BuildingTypes.First(x => x.Name == "Residential").Id;
        }

        var response = await Client.GetAsync($"/api/reports/policies-by-city?buildingTypeId={buildingTypeId}&page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResultDto<GetPoliciesByCityReportDto>>();

        result!.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task PoliciesByBroker_ShouldFilterByStatus()
    {
        await SeedReportsData();
        await LoginAsAdmin();

        var response = await Client.GetAsync("/api/reports/policies-by-broker?status=Cancelled&page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResultDto<GetPoliciesByBrokerReportDto>>();

        result!.Items.Should().HaveCount(1);
        result.Items[0].PolicyCount.Should().Be(1);
    }

    [Fact]
    public async Task PoliciesByCountry_ShouldReturnEmpty_WhenOutsideDateRange()
    {
        await SeedReportsData();
        await LoginAsAdmin();

        var response = await Client.GetAsync("/api/reports/policies-by-country?from=2030-01-01&to=2030-12-31&page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResultDto<GetPoliciesByCountryReportDto>>();

        result!.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task PoliciesByCountry_ShouldReturn400_WhenInvalidDateRange()
    {
        await SeedReportsData();
        await LoginAsAdmin();

        var response = await Client.GetAsync("/api/reports/policies-by-country?from=2025-12-01&to=2025-01-01&page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
