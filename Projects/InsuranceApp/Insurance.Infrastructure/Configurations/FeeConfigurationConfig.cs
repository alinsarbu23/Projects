using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class FeeConfigurationConfig : IEntityTypeConfiguration<FeeConfiguration>
{
    public void Configure(EntityTypeBuilder<FeeConfiguration> entity)
    {
        entity.ToTable("FeeConfigurations");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Name)
              .IsRequired()
              .HasMaxLength(200);

        entity.Property(x => x.Type)
              .IsRequired()
              .HasMaxLength(50);

        entity.Property(x => x.Percentage)
              .HasColumnType("decimal(5,2)");

        entity.Property(x => x.AppliesToRisk)
              .HasMaxLength(20);

        entity.Property(x => x.IsActive)
              .IsRequired();

        entity.Property(x => x.EffectiveFrom)
              .IsRequired();

        entity.Property(x => x.EffectiveTo);
    }
}
