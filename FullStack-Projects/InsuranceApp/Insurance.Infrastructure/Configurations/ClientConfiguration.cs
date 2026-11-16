using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> entity)
    {
        entity.HasKey(e => e.Id)
              .HasName("PK__Clients__3214EC07E0E04D9B");

        entity.Property(e => e.Id)
              .ValueGeneratedNever();

        entity.Property(e => e.ClientTypeId)
              .IsRequired();

        entity.HasOne(e => e.ClientType)
              .WithMany()
              .HasForeignKey(e => e.ClientTypeId)
              .OnDelete(DeleteBehavior.Restrict);

        entity.Property(e => e.Name)
              .IsRequired()
              .HasMaxLength(255);

        entity.Property(e => e.Email)
              .HasMaxLength(100);

        entity.Property(e => e.Phone)
              .HasMaxLength(50);

        entity.Property(e => e.Address)
              .HasMaxLength(255);

        entity.Property(e => e.Cnp)
              .HasMaxLength(13);

        entity.Property(e => e.Cui)
              .HasMaxLength(10);

        entity.Property(e => e.CreatedAt)
              .HasPrecision(0)
              .HasDefaultValueSql("(sysutcdatetime())");

        entity.Property(e => e.UpdatedAt)
              .HasPrecision(0)
              .HasDefaultValueSql("(sysutcdatetime())");

        entity.HasIndex(e => e.Cnp)
              .IsUnique()
              .HasFilter("[Cnp] IS NOT NULL")
              .HasDatabaseName("UX_Clients_Cnp");

        entity.HasIndex(e => e.Cui)
              .IsUnique()
              .HasFilter("[Cui] IS NOT NULL")
              .HasDatabaseName("UX_Clients_Cui");
    }
}
