using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public static readonly Guid RomaniaId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    public void Configure(EntityTypeBuilder<Country> entity)
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).HasMaxLength(100);
        entity.HasIndex(e => e.Name).IsUnique();

        entity.HasData(new Country
        {
            Id = RomaniaId,
            Name = "Romania"
        });
    }
}
