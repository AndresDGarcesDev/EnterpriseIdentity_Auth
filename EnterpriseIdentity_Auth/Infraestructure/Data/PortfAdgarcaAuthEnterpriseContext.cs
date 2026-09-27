using System;
using System.Collections.Generic;
using EnterpriseIdentity_Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseIdentity_Auth.Infraestructure.Data;

public partial class PortfAdgarcaAuthEnterpriseContext : DbContext
{
    public PortfAdgarcaAuthEnterpriseContext()
    {
    }

    public PortfAdgarcaAuthEnterpriseContext(DbContextOptions<PortfAdgarcaAuthEnterpriseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectingDefault");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC079C1E2B71");

            entity.Property(e => e.Browser).HasMaxLength(100);
            entity.Property(e => e.CreatedByIp).HasMaxLength(100);
            entity.Property(e => e.DeviceType).HasMaxLength(50);
            entity.Property(e => e.LastActivity).HasColumnType("datetime");
            entity.Property(e => e.OperatingSystem).HasMaxLength(100);
            entity.Property(e => e.ReplacedByToken).HasMaxLength(200);
            entity.Property(e => e.RevokedByIp).HasMaxLength(100);
            entity.Property(e => e.Token).HasMaxLength(200);

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefreshTo__UserI__3C69FB99");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07FBB77F38");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07D2408291");

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Roles).WithMany(p => p.Users)
                .HasForeignKey(d => d.RolesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RolesId__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
