using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> entity)
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.Name)
              .IsRequired()
              .HasMaxLength(100);

        entity.Property(x => x.Code)
              .IsRequired()
              .HasMaxLength(20);

        entity.HasIndex(x => new { x.CountyId, x.Code })
              .IsUnique();

        entity.HasOne(x => x.County)
              .WithMany(c => c.Cities)
              .HasForeignKey(x => x.CountyId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}
