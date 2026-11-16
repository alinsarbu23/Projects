using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class RiskFactorConfigurationConfig : IEntityTypeConfiguration<RiskFactorConfiguration>
{
    public void Configure(EntityTypeBuilder<RiskFactorConfiguration> entity)
    {
        entity.ToTable("RiskFactorConfigurations");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.Level)
              .IsRequired()
              .HasMaxLength(20);

        entity.Property(x => x.AdjustmentPercentage)
              .HasColumnType("decimal(5,2)");

        entity.Property(x => x.IsActive)
              .IsRequired();

        entity.HasIndex(x => new { x.Level, x.ReferenceId });
    }
}
