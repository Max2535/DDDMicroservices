using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using inventory_management.Models;

namespace inventory_management.Data;

public partial class InventDbContext : DbContext
{
    public InventDbContext()
    {
    }

    public InventDbContext(DbContextOptions<InventDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<product> products { get; set; }

    public virtual DbSet<stock_balance> stock_balances { get; set; }

    public virtual DbSet<stock_movement> stock_movements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=inventory_db;Username=admin;Password=yourpassword");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<product>(entity =>
        {
            entity.HasKey(e => e.id).HasName("products_pkey");

            entity.HasIndex(e => e.sku, "products_sku_key").IsUnique();

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.name).HasMaxLength(255);
            entity.Property(e => e.price).HasPrecision(10, 2);
            entity.Property(e => e.sku).HasMaxLength(50);
        });

        modelBuilder.Entity<stock_balance>(entity =>
        {
            entity.HasKey(e => e.id).HasName("stock_balance_pkey");

            entity.ToTable("stock_balance");

            entity.Property(e => e.updated_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.product).WithMany(p => p.stock_balances)
                .HasForeignKey(d => d.product_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stock_balance_product_id_fkey");
        });

        modelBuilder.Entity<stock_movement>(entity =>
        {
            entity.HasKey(e => e.id).HasName("stock_movements_pkey");

            entity.Property(e => e.created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.movement_type).HasMaxLength(20);

            entity.HasOne(d => d.product).WithMany(p => p.stock_movements)
                .HasForeignKey(d => d.product_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stock_movements_product_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
