using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Infrastructure.Configurations;

public class ClientTypeConfiguration : IEntityTypeConfiguration<ClientType>
{
    public void Configure(EntityTypeBuilder<ClientType> entity)
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.Code)
              .IsRequired()
              .HasMaxLength(20);

        entity.Property(x => x.Name)
              .IsRequired()
              .HasMaxLength(100);

        entity.HasIndex(x => x.Code)
              .IsUnique();
    }
}
