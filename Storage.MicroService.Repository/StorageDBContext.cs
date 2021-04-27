using Microsoft.EntityFrameworkCore;
using Storage.MicroService.Models;
using System;

namespace Storage.MicroService.Repository
{
    public partial class StorageDBContext : DbContext
    {
        public StorageDBContext()
        {
        }

        public StorageDBContext(DbContextOptions<StorageDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<StorageEntity> Storages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("data source=192.168.217.129;initial catalog=storagedb;user id=root;password=123456", Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.21-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StorageEntity>(entity =>
            {
                entity.ToTable("storage");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductId")
                    .HasComment("产品id");

                entity.Property(e => e.Residue)
                    .HasColumnName("residue")
                    .HasComment("剩余库存");

                entity.Property(e => e.Total)
                    .HasColumnName("total")
                    .HasComment("总库存");

                entity.Property(e => e.Used)
                    .HasColumnName("used")
                    .HasComment("已用库存");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

