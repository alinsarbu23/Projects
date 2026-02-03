using Insurance.Infrastructure;
using Insurance.Infrastructure.DAOs;
using Microsoft.AspNetCore.Identity;

namespace Insurance.Infrastructure.Seed;
public static class DatabaseSeeder
{
    public static async Task SeedAsync(
        InsuranceDbContext db,
        RoleManager<IdentityRole>? roleManager)
    {

        if (roleManager != null)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Broker"))
            {
                await roleManager.CreateAsync(new IdentityRole("Broker"));
            }
        }

        if (!db.ClientTypes.Any())
        {
            db.ClientTypes.AddRange(
                new ClientType
                {
                    Code = "INDIVIDUAL",
                    Name = "Individual"
                },
                new ClientType
                {
                    Code = "COMPANY",
                    Name = "Company"
                }
            );

            await db.SaveChangesAsync();
        }

        if (!db.Countries.Any())
        {
            var countryId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var countyId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
            var cityId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

            db.Countries.Add(new Country
            {
                Id = countryId,
                Name = "Romania"
            });

            db.Counties.Add(new County
            {
                Id = countyId,
                CountryId = countryId,
                Name = "Bucharest"
            });

            db.Cities.Add(new City
            {
                Id = cityId,
                CountyId = countyId,
                Name = "Bucharest",
                Code = "RO-B"
            });

            await db.SaveChangesAsync();
        }

        if (!db.BuildingTypes.Any())
        {
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
                },
                new BuildingType
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Industrial"
                }
            );

            await db.SaveChangesAsync();
        }
    }
}
