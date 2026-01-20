using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class BuildingConfiguration : IEntityTypeConfiguration<Building>
{
    public void Configure(EntityTypeBuilder<Building> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("PK__Building__3214EC07F1EBAC51");

        entity.HasIndex(e => e.ClientId)
              .HasDatabaseName("IX_Buildings_ClientId");

        entity.HasIndex(e => e.CityId)
              .HasDatabaseName("IX_Buildings_CityId");

        entity.HasIndex(e => e.BuildingTypeId)
              .HasDatabaseName("IX_Buildings_BuildingTypeId");

        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.Street).HasMaxLength(255);
        entity.Property(e => e.StreetNumber).HasMaxLength(50);

        entity.Property(e => e.SurfaceArea)
              .HasColumnType("decimal(10,2)");

        entity.Property(e => e.InsuredValue)
              .HasColumnType("decimal(15,2)");

        entity.Property(e => e.CreatedAt)
              .HasPrecision(0)
              .HasDefaultValueSql("(sysutcdatetime())");

        entity.Property(e => e.UpdatedAt)
              .HasPrecision(0)
              .HasDefaultValueSql("(sysutcdatetime())");

        entity.HasOne(d => d.Client)
              .WithMany(p => p.Buildings)
              .HasForeignKey(d => d.ClientId)
              .HasConstraintName("FK_Buildings_Clients");

        entity.HasOne(d => d.City)
              .WithMany(p => p.Buildings)
              .HasForeignKey(d => d.CityId)
              .HasConstraintName("FK_Buildings_Cities");

        entity.HasOne(d => d.BuildingType)
              .WithMany(p => p.Buildings)
              .HasForeignKey(d => d.BuildingTypeId)
              .HasConstraintName("FK_Buildings_BuildingTypes");
    }
}
