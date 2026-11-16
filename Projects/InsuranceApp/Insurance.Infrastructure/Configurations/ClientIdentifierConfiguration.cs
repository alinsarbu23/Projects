using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Client = Insurance.Infrastructure.DAOs.Client;
using ClientIdentifier = Insurance.Infrastructure.DAOs.ClientIdentifier;

namespace Insurance.Infrastructure.Configurations;

public class ClientIdentifierConfiguration : IEntityTypeConfiguration<ClientIdentifier>
{
    public void Configure(EntityTypeBuilder<ClientIdentifier> builder)
    {
        builder.ToTable("ClientIdentifier");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OldIdentifier)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.NewIdentifier)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.IdentifierType)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.ChangedAt)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne<Client>()
            .WithMany()
            .HasForeignKey(x => x.ClientId);
    }
}
