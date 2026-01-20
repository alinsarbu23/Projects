using Insurance.Infrastructure.Configurations;
using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure;

public partial class InsuranceDbContext : DbContext
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BuildingConfiguration());
        modelBuilder.ApplyConfiguration(new BuildingTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
        modelBuilder.ApplyConfiguration(new CountyConfiguration());
        modelBuilder.ApplyConfiguration(new CountryConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
