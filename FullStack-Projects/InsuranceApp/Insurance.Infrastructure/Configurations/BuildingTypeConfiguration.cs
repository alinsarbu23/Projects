using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class BuildingTypeConfiguration : IEntityTypeConfiguration<BuildingType>
{
    public void Configure(EntityTypeBuilder<BuildingType> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("PK__Building__3214EC0737A90D86");

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.Name).HasMaxLength(50);

        entity.HasIndex(e => e.Name)
              .IsUnique()
              .HasDatabaseName("UX_BuildingTypes_Name");
    }
}
