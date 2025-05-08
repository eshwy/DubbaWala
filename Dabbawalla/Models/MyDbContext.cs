using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Dabbawalla.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<ContactUs> ContactUs { get; set; }

    public virtual DbSet<DeliveryStatus> DeliveryStatuses { get; set; }

    public virtual DbSet<FoodItem> FoodItems { get; set; }

    public virtual DbSet<FoodType> FoodTypes { get; set; }

    public virtual DbSet<MealTypeProvided> MealTypeProvideds { get; set; }

    public virtual DbSet<MenuType> MenuTypes { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<PaymentDetail> PaymentDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VendorWorkingDay> VendorWorkingDays { get; set; }

    public virtual DbSet<WorkingDay> WorkingDays { get; set; }
    public virtual DbSet<FeedBack> FeedBack { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=mysql-myproj-eshwanth.d.aivencloud.com;port=19981;Database=dubbawala;user=avnadmin;password=AVNS_ZM1w0zcXNv6XAuX3T8w;sslmode=Preferred", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Address");

            entity.Property(e => e.AddressType).HasMaxLength(50);
            entity.Property(e => e.Area).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.DoorNumber).HasMaxLength(50);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.State).HasMaxLength(255);
            entity.Property(e => e.Street).HasMaxLength(255);
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
        });
        modelBuilder.Entity<FeedBack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<ContactUs>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Message).HasMaxLength(300);
            entity.Property(e => e.Subject).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(36);
        });

        modelBuilder.Entity<DeliveryStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("DeliveryStatus");

            entity.HasIndex(e => e.OrderId, "OrderId");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.DeliveryStatuses)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("DeliveryStatus_ibfk_1");
        });

        modelBuilder.Entity<FoodItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("FoodItem");

            entity.HasIndex(e => e.FoodId, "FoodId");

            entity.HasIndex(e => e.MenuId, "MenuId");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.Property(e => e.Rating).HasPrecision(10);
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(36);

            entity.HasOne(d => d.Food).WithMany(p => p.FoodItems)
                .HasForeignKey(d => d.FoodId)
                .HasConstraintName("FoodItem_ibfk_1");

            entity.HasOne(d => d.Menu).WithMany(p => p.FoodItems)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("FoodItem_ibfk_2");
        });

        modelBuilder.Entity<FoodType>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PRIMARY");

            entity.ToTable("FoodType");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<MealTypeProvided>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("MealTypeProvided");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(3);
        });

        modelBuilder.Entity<MenuType>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PRIMARY");

            entity.ToTable("MenuType");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.AddressId, "AddressId");

            entity.HasIndex(e => e.FoodId, "FoodId");

            entity.HasIndex(e => e.PaymentDetailsId, "PaymentDetailsId");

            entity.Property(e => e.Completed).HasDefaultValueSql("'0'");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .HasColumnName("userId");

            entity.HasOne(d => d.PaymentDetails).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.PaymentDetailsId)
                .HasConstraintName("OrderDetails_ibfk_4");
        });

        modelBuilder.Entity<PaymentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.OrderId, "OrderId");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMode).HasMaxLength(50);
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.Property(e => e.TransactionStatus).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            //entity.HasOne(d => d.IdNavigation).WithOne(p => p.PaymentDetail)
            //    .HasForeignKey<PaymentDetail>(d => d.Id)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("PaymentDetails_ibfk_1");

            //entity.HasOne(d => d.Order).WithMany(p => p.PaymentDetailsNavigation)
            //    .HasForeignKey(d => d.OrderId)
            //    .HasConstraintName("PaymentDetails_ibfk_2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("Role");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.RoleDescription).HasColumnType("text");
            entity.Property(e => e.RoleName).HasMaxLength(255);
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User");

            entity.HasIndex(e => e.RoleId, "RoleId");

            entity.Property(e => e.BankAccountNumber).HasMaxLength(50);
            entity.Property(e => e.BankIfsc).HasMaxLength(50);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.EmailAddress).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Pancard).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(10);
            entity.Property(e => e.RestrauntName)
                .HasMaxLength(50)
                .HasColumnName("Restraunt_Name");
            entity.Property(e => e.RestrauntName1)
                .HasMaxLength(30)
                .HasColumnName("RestrauntName");
            entity.Property(e => e.UpdatedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("User_ibfk_1");
        });

        modelBuilder.Entity<VendorWorkingDay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.UserId).HasMaxLength(3);
        });

        modelBuilder.Entity<WorkingDay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Day).HasMaxLength(15);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
