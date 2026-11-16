using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Insurance.Infrastructure.DAOs;

namespace Insurance.Infrastructure.Configurations;

public class BrokerConfiguration : IEntityTypeConfiguration<Broker>
{
    public void Configure(EntityTypeBuilder<Broker> entity)
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.BrokerCode)
              .IsRequired()
              .HasMaxLength(50);

        entity.HasIndex(x => x.BrokerCode)
              .IsUnique();

        entity.Property(x => x.Name)
              .IsRequired()
              .HasMaxLength(200);

        entity.Property(x => x.CommissionPercentage)
              .HasColumnType("decimal(5,2)");

        entity.Property(x => x.Status)
              .IsRequired()
              .HasMaxLength(20);
    }
}
