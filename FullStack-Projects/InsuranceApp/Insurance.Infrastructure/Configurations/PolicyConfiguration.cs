using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class PolicyConfiguration : IEntityTypeConfiguration<Policy>
{
    public void Configure(EntityTypeBuilder<Policy> entity)
    {
        entity.ToTable("Policies");

        entity.HasKey(x => x.Id);

        entity.Property(x => x.PolicyNumber)
              .IsRequired()
              .HasMaxLength(50);

        entity.HasIndex(x => x.PolicyNumber)
              .IsUnique();

        entity.Property(x => x.Status)
              .IsRequired()
              .HasMaxLength(20);

        entity.Property(x => x.BasePremium)
              .HasColumnType("decimal(15,2)");

        entity.Property(x => x.FinalPremium)
              .HasColumnType("decimal(15,2)");

        entity.Property(x => x.CancellationReason)
              .HasMaxLength(255);

        entity.HasOne(x => x.Broker)
              .WithMany(b => b.Policies)
              .HasForeignKey(x => x.BrokerId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(x => x.BuildingId);
        entity.HasIndex(x => x.CurrencyId);
    }
}
