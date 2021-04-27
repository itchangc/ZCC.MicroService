using Order.MicroService.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Order.MicroService.Repository
{
    public partial class OrderDBContext : DbContext
    {
        public OrderDBContext()
        {
        }

        public OrderDBContext(DbContextOptions<OrderDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<OrderEntity> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("data source=192.168.217.129;initial catalog=orderdb;user id=root;password=123456", Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.21-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderEntity>(entity =>
            {
                entity.ToTable("order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Count)
                    .HasColumnName("Count")
                    .HasComment("数量");

                entity.Property(e => e.Money)
                    .HasPrecision(11)
                    .HasColumnName("Money")
                    .HasComment("金额");

                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductId")
                    .HasComment("产品id");

                entity.Property(e => e.Status)
                    .HasColumnName("Status")
                    .HasComment("订单状态：0：创建中；1：已完结");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserId")
                    .HasComment("用户id");

                entity.Property(e => e.CreateTime)
                   .HasColumnName("CreateTime")
                   .HasComment("创建时间");

                
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
