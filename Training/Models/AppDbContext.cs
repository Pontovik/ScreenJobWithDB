using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScreenJob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenJob
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<BuyProduct> BuyProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Data Source=ZVERZVE-3DSOVRD;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Database=testDB");
            optionsBuilder.UseSqlServer(@"Data Source=ZVERZVE-3DSOVRD;Database=testDB;Trusted_Connection=True;Integrated Security=True;TrustServerCertificate=False;Encrypt=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(UserConfigure);
            modelBuilder.Entity<Product>(ProductConfigure);
            modelBuilder.Entity<Order>(OrderConfigure);
            modelBuilder.Entity<BuyProduct>(BuyProductConfigure);
        }

        protected void UserConfigure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.UserId);
            builder.ToTable("User");
            builder.Property(e => e.UserId).HasColumnName("user_id");
            builder.Property(e => e.UserEmail).HasColumnName("user_email");
            builder.Property(e => e.UserLogin).HasColumnName("user_login");
            builder.Property(e => e.UserRegistrationDatetime).HasColumnName("user_registration_datetime");
            builder.Property(e => e.UserName).HasColumnName("user_name");
            builder.Property(e => e.UserPhone).HasColumnName("user_phone");
            builder.Property(e => e.UserIsDelete).HasColumnName("user_is_delete");
            builder.HasMany(u => u.Orders).WithOne(o => o.User);
        }

        protected void ProductConfigure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.ProductId);
            builder.ToTable("Product");
            builder.Property(e => e.ProductId).HasColumnName("product_id");
            builder.Property(e => e.ProductPrice).HasColumnName("product_price");
            builder.Property(e => e.ProductAmount).HasColumnName("product_amount");
            builder.Property(e => e.ProductName).HasColumnName("product_name");
            builder.Property(e => e.ProductIsDelete).HasColumnName("product_is_delete");
        }

        protected void OrderConfigure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.OrderId);
            builder.ToTable("Order");
            builder.Property(e => e.OrderId).HasColumnName("order_id");
            builder.Property(e => e.OrderSum).HasColumnName("order_sum");
            builder.Property(e => e.OrderRegDate).HasColumnName("order_reg_date");
            builder.Property(e => e.OrderNumber).HasColumnName("order_number");
            builder.Property(e => e.UserId).HasColumnName("user_id");
            builder.HasOne(e => e.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId);
        }

        protected void BuyProductConfigure(EntityTypeBuilder<BuyProduct> builder)
        {
            builder.HasKey(e => e.BuyproductId);
            builder.ToTable("BuyProduct");
            builder.Property(e => e.BuyproductId).HasColumnName("buyproduct_id");
            builder.Property(e => e.BuyproductAmount).HasColumnName("buyproduct_amount");
            builder.Property(e => e.BuyproductPrice).HasColumnName("buyproduct_price");
            builder.Property(e => e.OrderId).HasColumnName("order_id");
            builder.Property(e => e.ProductId).HasColumnName("product_id");
            builder.HasOne(d => d.Order).WithMany(p => p.BuyProducts).HasForeignKey(d => d.OrderId);
            builder.HasOne(d => d.Product).WithMany(p => p.BuyProducts).HasForeignKey(d => d.ProductId);
        }
    }
}