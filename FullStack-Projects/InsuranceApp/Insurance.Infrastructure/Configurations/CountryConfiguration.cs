using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> entity)
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.Name)
              .IsRequired()
              .HasMaxLength(100);

        entity.HasIndex(x => x.Name)
              .IsUnique();
    }
}
