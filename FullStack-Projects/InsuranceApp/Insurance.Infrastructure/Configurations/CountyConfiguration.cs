using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class CountyConfiguration : IEntityTypeConfiguration<County>
{
    public void Configure(EntityTypeBuilder<County> entity)
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.Name)
              .IsRequired()
              .HasMaxLength(100);

        entity.HasIndex(x => new { x.CountryId, x.Name })
              .IsUnique();

        entity.HasOne(x => x.Country)
              .WithMany(c => c.Counties)
              .HasForeignKey(x => x.CountryId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}
