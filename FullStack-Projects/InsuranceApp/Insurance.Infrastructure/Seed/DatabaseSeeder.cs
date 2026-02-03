using Insurance.Infrastructure;
using Insurance.Infrastructure.DAOs;
using Microsoft.AspNetCore.Identity;

namespace Insurance.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(
        InsuranceDbContext db,
        RoleManager<IdentityRole> roleManager)
    {

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            });
        }

        if (!await roleManager.RoleExistsAsync("Broker"))
        {
            await roleManager.CreateAsync(new IdentityRole
            {
                Name = "Broker",
                NormalizedName = "BROKER"
            });
        }

        if (!db.ClientTypes.Any())
        {
            db.ClientTypes.AddRange(
                new ClientType { Id = 1, Code = "INDIVIDUAL", Name = "Individual" },
                new ClientType { Id = 2, Code = "COMPANY", Name = "Company" }
            );
        }

        if (!db.Clients.Any())
        {
            db.Clients.Add(new Client
            {
                Id = Guid.NewGuid(),
                ClientTypeId = 1, 
                Name = "Test Client",
                Cnp = "1800101223344",
                Email = "test@insurance.ro",
                Phone = "0700000000",
                Address = "Test Address"
            });
        }

        await db.SaveChangesAsync();
    }
}
