﻿using DAL.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data
{
    public class EFAppContext : DbContext
    {
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductImageEntity> ProductImages { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BasketEntity> Baskets { get; set; }
        public DbSet<OrderStatusEntity> OrderStatuses { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<SaleEntity> Sales { get; set;  }
        public DbSet<Sales_ProductEntity> Sales_Products { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                //.UseNpgsql("Server=localhost;Port=5432;Database=revo;User Id=postgres;Password=123456;");
                .UseSqlite("DataSource=revo.db;");
            // .UseSqlServer("Server=.;Database=revo;Trusted_Connection=True;TrustServerCertificate=true;");
            //.UseSqlServer("Data Source=20.65.144.204;User ID=kaban;Password=9[nV`e7VN`0%;Initial Catalog=revo;MultipleActiveResultSets=true;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BasketEntity>(basket =>
            {
                basket.HasKey(b => new { b.UserId, b.ProductId });
            });

            modelBuilder.Entity<Sales_ProductEntity>(sales =>
            {
                sales.HasKey(b => new { b.SaleId, b.ProductId });
            });

            modelBuilder.Entity<UserRoleEntity>(ur =>
            {
                ur.HasKey(b => new { b.UserId, b.RoleId });
            });

            // modelBuilder.Entity<CategoryEntity>().HasQueryFilter(p => !p.IsDelete);
            // modelBuilder.Entity<ProductEntity>().HasQueryFilter(p => !p.IsDelete);
            // modelBuilder.Entity<BasketEntity>().HasQueryFilter(p => !p.Product.IsDelete);
            // modelBuilder.Entity<ProductImageEntity>().HasQueryFilter(p => !p.IsDelete);
            // modelBuilder.Entity<UserEntity>().HasQueryFilter(p => !p.IsDelete);
            // modelBuilder.Entity<OrderStatusEntity>().HasQueryFilter(p => !p.IsDelete);
            // modelBuilder.Entity<OrderItemEntity>().HasQueryFilter(p => !p.IsDelete);
            // modelBuilder.Entity<OrderEntity>().HasQueryFilter(p => !p.IsDelete);
            // modelBuilder.Entity<SaleEntity>().HasQueryFilter(p => !p.IsDelete);
            // modelBuilder.Entity<Sales_ProductEntity>().HasQueryFilter(p => DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc) < p.Sale.ExpireTime);
            // modelBuilder.Entity<RoleEntity>().HasQueryFilter(p => !p.IsDelete);
            // modelBuilder.Entity<UserRoleEntity>().HasQueryFilter(p => !p.Role.IsDelete);
        }
    }
}
