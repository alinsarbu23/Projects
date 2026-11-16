using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Insurance.Infrastructure.DAOs;

namespace Insurance.Infrastructure.Configurations;

public class AdministratorConfiguration : IEntityTypeConfiguration<Administrator>
{
    public void Configure(EntityTypeBuilder<Administrator> entity)
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
        entity.Property(x => x.Email).IsRequired().HasMaxLength(100);
        entity.Property(x => x.Role).IsRequired().HasMaxLength(20);

        entity.HasIndex(x => x.Email).IsUnique();
    }
}
