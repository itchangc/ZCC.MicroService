using Account.MicroService.Models;
using Microsoft.EntityFrameworkCore;
namespace Account.MicroService.Repository
{
    public partial class AccountDBContext : DbContext
    {
        private DbSet<AccountEntity> accounts;

        public AccountDBContext()
        {
        }

        public AccountDBContext(DbContextOptions<AccountDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountEntity> Accounts { get => accounts; set => accounts = value; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("data source=192.168.217.129;initial catalog=accountdb;user id=root;password=123456", Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.21-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountEntity>(entity =>
            {
                entity.ToTable("account");

                entity.Property(e => e.Id)
                    .HasColumnName("Id")
                    .HasComment("Id");

                entity.Property(e => e.Residue)
                    .HasPrecision(10)
                    .HasColumnName("Residue")
                    .HasDefaultValueSql("'0'")
                    .HasComment("剩余可用额度");

                entity.Property(e => e.Total)
                    .HasPrecision(10)
                    .HasColumnName("Total")
                    .HasComment("总额度");

                entity.Property(e => e.Used)
                    .HasPrecision(10)
                    .HasColumnName("Used")
                    .HasComment("已用余额");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserId")
                    .HasComment("用户id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
