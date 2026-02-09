using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> entity)
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.Code)
              .IsRequired()
              .HasMaxLength(10);

        entity.HasIndex(x => x.Code).IsUnique();

        entity.Property(x => x.ExchangeRateToBase)
              .HasColumnType("decimal(10,4)");
    }
}
