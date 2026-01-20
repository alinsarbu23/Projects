using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public static readonly Guid BucharestCityId =
        Guid.Parse("33333333-3333-3333-3333-333333333333");

    public void Configure(EntityTypeBuilder<City> entity)
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).HasMaxLength(100);
        entity.Property(e => e.Code).HasMaxLength(20);

        entity.HasIndex(e => new { e.CountyId, e.Code }).IsUnique();

        entity.HasData(new City
        {
            Id = BucharestCityId,
            CountyId = CountyConfiguration.BucharestCountyId,
            Name = "Bucharest",
            Code = "RO-B"
        });
    }
}
