using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class PolicyConfiguration : IEntityTypeConfiguration<Policy>
{
    public void Configure(EntityTypeBuilder<Policy> entity)
    {
        entity.ToTable("Policies");

        entity.HasKey(p => p.Id);

        entity.Property(p => p.PolicyNumber)
              .IsRequired()
              .HasMaxLength(50);

        entity.HasIndex(p => p.PolicyNumber)
              .IsUnique();

        entity.Property(p => p.Status)
              .IsRequired()
              .HasMaxLength(20);

        entity.Property(p => p.BasePremium)
              .HasColumnType("decimal(15,2)");

        entity.Property(p => p.FinalPremium)
              .HasColumnType("decimal(15,2)");

        entity.Property(p => p.CancellationReason)
              .HasMaxLength(255);

        entity.HasOne(p => p.Building)
              .WithMany()
              .HasForeignKey(p => p.BuildingId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(p => p.Broker)
              .WithMany(b => b.Policies)
              .HasForeignKey(p => p.BrokerId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(p => p.Currency)
              .WithMany()
              .HasForeignKey(p => p.CurrencyId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}
