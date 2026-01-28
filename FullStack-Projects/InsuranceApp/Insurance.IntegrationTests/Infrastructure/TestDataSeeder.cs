using Insurance.Infrastructure;
using Insurance.Infrastructure.DAOs;

namespace Insurance.IntegrationTests.Infrastructure;

public static class TestDataSeeder
{
    public static async Task SeedAsync(InsuranceDbContext db)
    {
        if (db.Countries.Any())
            return;

        var romaniaId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var bucharestCountyId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var bucharestCityId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

        db.Countries.Add(new Country
        {
            Id = romaniaId,
            Name = "Romania"
        });

        db.Counties.Add(new County
        {
            Id = bucharestCountyId,
            CountryId = romaniaId,
            Name = "Bucharest"
        });

        db.Cities.Add(new City
        {
            Id = bucharestCityId,
            CountyId = bucharestCountyId,
            Name = "Bucharest",
            Code = "B"
        });

        db.BuildingTypes.AddRange(
            new BuildingType
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Residential"
            },
            new BuildingType
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Office"
            }
        );

        await db.SaveChangesAsync();
    }
}
