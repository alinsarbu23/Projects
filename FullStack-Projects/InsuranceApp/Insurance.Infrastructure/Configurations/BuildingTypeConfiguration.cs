using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class BuildingTypeConfiguration : IEntityTypeConfiguration<BuildingType>
{
    public void Configure(EntityTypeBuilder<BuildingType> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
              .IsRequired()
              .HasMaxLength(50);

        entity.HasIndex(e => e.Name)
              .IsUnique();

        entity.HasData(
            new BuildingType
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Residential"
            },
            new BuildingType
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Office"
            },
            new BuildingType
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Industrial"
            }
        );
    }
}
