using Insurance.Infrastructure;
using Insurance.Infrastructure.DAOs;
using Insurance.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Infrastructure.Seed;

public static class DatabaseSeeder
{
    private const string AdminEmail = "admin@insurance.local";
    private const string AdminPassword = "Admin123!";

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<InsuranceDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApiUser>>();

        if (!await roleManager.RoleExistsAsync(Roles.Admin))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
        }

        if (!await roleManager.RoleExistsAsync(Roles.Broker))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Broker));
        }

        var adminUser = await userManager.FindByEmailAsync(AdminEmail);
        if (adminUser == null)
        {
            adminUser = new ApiUser
            {
                UserName = AdminEmail,
                Email = AdminEmail
            };

            await userManager.CreateAsync(adminUser, AdminPassword);
            await userManager.AddToRoleAsync(adminUser, Roles.Admin);
        }

        if (!db.Administrators.Any())
        {
            db.Administrators.Add(new Administrator
            {
                Id = Guid.NewGuid(),
                Name = "System Admin",
                Email = AdminEmail,
                Role = Roles.Admin
            });

            await db.SaveChangesAsync();
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
