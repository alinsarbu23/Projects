using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class CountyConfiguration : IEntityTypeConfiguration<County>
{
    public static readonly Guid BucharestCountyId =
        Guid.Parse("22222222-2222-2222-2222-222222222222");

    public void Configure(EntityTypeBuilder<County> entity)
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).HasMaxLength(100);

        entity.HasIndex(e => new { e.CountryId, e.Name }).IsUnique();

        entity.HasData(new County
        {
            Id = BucharestCountyId,
            CountryId = CountryConfiguration.RomaniaId,
            Name = "Bucharest"
        });
    }
}
