using Insurance.Infrastructure.Configurations;
using Insurance.Infrastructure.DAOs;
using Insurance.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<County> Counties => Set<County>();
    public DbSet<ClientIdentifier> ClientIdentifiers => Set<ClientIdentifier>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CountryConfiguration());
        modelBuilder.ApplyConfiguration(new CountyConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
        modelBuilder.ApplyConfiguration(new BuildingConfiguration());
        modelBuilder.ApplyConfiguration(new BuildingTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new ClientIdentifierConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
    }
}
