using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Project1.StoreApplication.Storage.DBModels;

#nullable disable

namespace Project1.StoreApplication.Storage
{
    public partial class StoreApplicationDB2Context : DbContext
    {
        public StoreApplicationDB2Context()
        {
        }

        private readonly string _connStr = "";
        public StoreApplicationDB2Context(DbContextOptions<StoreApplicationDB2Context> options, IConfiguration config, ILogger<StoreApplicationDB2Context> logger)
            : base(options)
        {
            _connStr = config.GetConnectionString("StoreApplicationDB2");
        }

        public virtual DbSet<DBCustomer> Customers { get; set; }
        public virtual DbSet<DBOrder> Orders { get; set; }
        public virtual DbSet<DBOrderProduct> OrderProducts { get; set; }
        public virtual DbSet<DBProduct> Products { get; set; }
        public virtual DbSet<DBProductCategory> ProductCategories { get; set; }
        public virtual DbSet<DBStore> Stores { get; set; }
        public virtual DbSet<DBStoreProduct> StoreProducts { get; set; }
        public virtual DbSet<DBCustomerLogin> CustomerLogins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connStr);
                // TODO: use user secrets
                //optionsBuilder.UseSqlServer()
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<DBCustomerLogin>(entity =>
            {
                entity.ToTable("Customer", "CustomerLogin");

                entity.HasKey(e => e.CustomerLoginId);
                entity.Property(e => e.CustomerLoginId).HasColumnName("CustomerLoginID");

                entity.HasOne(e => e.Customer)
                    .WithOne(c => c.CustomerLogin)
                    .HasForeignKey<DBCustomerLogin>(e => e.CustomerId)
                    .HasConstraintName("FK_CustomerLogin_Customer");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

            });

            modelBuilder.Entity<DBCustomer>(entity =>
            {
                entity.ToTable("Customer", "Customer");

                entity.HasKey(e => e.CustomerId);
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.DefaultStoreNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.DefaultStore)
                    .HasConstraintName("FK_Customer_DefaultStore");
            });

            modelBuilder.Entity<DBOrder>(entity =>
            {
                entity.ToTable("Order", "Store");

                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Customer");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Store");
            });

            modelBuilder.Entity<DBOrderProduct>(entity =>
            {
                entity.ToTable("OrderProduct", "Store");

                entity.HasKey(e => e.OrderProductId);
                entity.Property(e => e.OrderProductId).HasColumnName("OrderProductID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderProduct_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderProduct_Product");
            });

            modelBuilder.Entity<DBProduct>(entity =>
            {
                entity.ToTable("Product", "Store");

                entity.HasKey(e => e.ProductId);
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Product_Category");
            });

            modelBuilder.Entity<DBProductCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK_Category");

                entity.ToTable("ProductCategory", "Store");

                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<DBStore>(entity =>
            {
                entity.ToTable("Store", "Store");

                entity.HasKey(e => e.StoreId);
                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<DBStoreProduct>(entity =>
            {
                entity.ToTable("StoreProduct", "Store");

                entity.HasKey(e => e.StoreProductId);
                entity.Property(e => e.StoreProductId).HasColumnName("StoreProductID");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.StoreProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreProduct_Product");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.StoreProducts)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoreProduct_Store");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
