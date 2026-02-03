using Insurance.Infrastructure.Configurations;
using Insurance.Infrastructure.DAOs;
using Insurance.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Insurance.Infrastructure;

public partial class InsuranceDbContext : IdentityDbContext<ApiUser>
{
    public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Building> Buildings => Set<Building>();
    public DbSet<BuildingType> BuildingTypes => Set<BuildingType>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<ClientType> ClientTypes => Set<ClientType>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<County> Counties => Set<County>();
    public DbSet<ClientIdentifier> ClientIdentifiers => Set<ClientIdentifier>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new CountryConfiguration());
        builder.ApplyConfiguration(new CountyConfiguration());
        builder.ApplyConfiguration(new CityConfiguration());
        builder.ApplyConfiguration(new BuildingConfiguration());
        builder.ApplyConfiguration(new BuildingTypeConfiguration());
        builder.ApplyConfiguration(new ClientConfiguration());
        builder.ApplyConfiguration(new ClientIdentifierConfiguration());
        builder.ApplyConfiguration(new ClientTypeConfiguration());
    }
}
