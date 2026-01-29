using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Insurance.Infrastructure;
using Insurance.Infrastructure.Repositories;
using Insurance.UnitTests.Helpers;

namespace Insurance.UnitTests.Repositories;

public class GeographyRepositoryTests
{
    private static InsuranceDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<InsuranceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InsuranceDbContext(options);
    }

    [Fact]
    public async Task GetAllCountriesAsync_ShouldReturnCountries()
    {
        using var context = CreateDbContext();
        var repo = new GeographyRepository(context, MapperFactory.Create());

        context.Countries.AddRange(
            new Infrastructure.DAOs.Country
            {
                Id = Guid.NewGuid(),
                Name = "Romania"
            },
            new Infrastructure.DAOs.Country
            {
                Id = Guid.NewGuid(),
                Name = "Austria"
            });

        await context.SaveChangesAsync();

        var result = await repo.GetAllCountriesAsync();

        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Austria");
        result[1].Name.Should().Be("Romania");
    }

    [Fact]
    public async Task GetCitiesByCountyIdAsync_ShouldReturnCities()
    {
        using var context = CreateDbContext();
        var repo = new GeographyRepository(context, MapperFactory.Create());

        var countyId = Guid.NewGuid();

        context.Cities.Add(new Infrastructure.DAOs.City
        {
            Id = Guid.NewGuid(),
            CountyId = countyId,
            Name = "Cluj-Napoca",
            Code = "CJ"
        });

        await context.SaveChangesAsync();

        var result = await repo.GetCitiesByCountyIdAsync(countyId);

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Cluj-Napoca");
    }

    [Fact]
    public async Task GetCountiesByCountryIdAsync_ShouldReturnRomania()
    {
        using var context = CreateDbContext();
        var repo = new GeographyRepository(context, MapperFactory.Create());

        var countryId = Guid.NewGuid();

        context.Counties.AddRange(
            new Infrastructure.DAOs.County { Id = Guid.NewGuid(), CountryId = countryId, Name = "B" },
            new Infrastructure.DAOs.County { Id = Guid.NewGuid(), CountryId = countryId, Name = "A" }
        );

        await context.SaveChangesAsync();

        var result = await repo.GetCountiesByCountryIdAsync(countryId);

        result.Should().HaveCount(2);
        result.Select(x => x.Name)
              .Should()
              .Contain(new[] { "A", "B" });

    }
}
