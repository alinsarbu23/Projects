using System;
using System.Collections.Generic;
using Insurance.Infrastructure.DAOs;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure;

public partial class InsuranceDbContext : DbContext
{
    public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<BuildingType> BuildingTypes { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<CompanyClient> CompanyClients { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<County> Counties { get; set; }

    public virtual DbSet<IndividualClient> IndividualClients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Building__3214EC07EEAE034F");

            entity.HasIndex(e => e.BuildingTypeId, "IX_Buildings_BuildingTypeId");

            entity.HasIndex(e => e.CityId, "IX_Buildings_CityId");

            entity.HasIndex(e => e.ClientId, "IX_Buildings_ClientId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.InsuredValue).HasColumnType("decimal(15, 2)");
            entity.Property(e => e.Street).HasMaxLength(255);
            entity.Property(e => e.StreetNumber).HasMaxLength(50);
            entity.Property(e => e.SurfaceArea).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.BuildingType).WithMany(p => p.Buildings)
                .HasForeignKey(d => d.BuildingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Buildings_BuildingTypes");

            entity.HasOne(d => d.City).WithMany(p => p.Buildings)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Buildings_Cities");

            entity.HasOne(d => d.Client).WithMany(p => p.Buildings)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Buildings_Clients");
        });

        modelBuilder.Entity<BuildingType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Building__3214EC07FC579CB9");

            entity.HasIndex(e => e.Name, "UQ__Building__737584F687454467").IsUnique();

            entity.HasIndex(e => e.Name, "UX_BuildingTypes_Name").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cities__3214EC0792C60FD6");

            entity.HasIndex(e => new { e.CountyId, e.Code }, "UX_Cities_County_Code").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.County).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cities_Counties");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clients__3214EC07E6BCC6D8");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ClientType).HasMaxLength(20);
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<CompanyClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CompanyC__3214EC072D0EBEDE");

            entity.HasIndex(e => e.Cui, "UX_CompanyClients_Cui").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CompanyName).HasMaxLength(255);
            entity.Property(e => e.Cui).HasMaxLength(10);

            entity.HasOne(d => d.Client).WithMany(p => p.CompanyClients)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanyClients_Clients");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Countrie__3214EC0755AB40B5");

            entity.HasIndex(e => e.Name, "UQ__Countrie__737584F68543F973").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<County>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Counties__3214EC0752AACDA3");

            entity.HasIndex(e => new { e.CountryId, e.Name }, "UX_Counties_Country_Name").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Country).WithMany(p => p.Counties)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Counties_Countries");
        });

        modelBuilder.Entity<IndividualClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Individu__3214EC07B19DA7DF");

            entity.HasIndex(e => e.Cnp, "UX_IndividualClients_Cnp").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cnp).HasMaxLength(13);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);

            entity.HasOne(d => d.Client).WithMany(p => p.IndividualClients)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IndividualClients_Clients");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
