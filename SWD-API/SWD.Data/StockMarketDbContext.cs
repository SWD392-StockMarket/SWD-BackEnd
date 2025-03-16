using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWD.Data.Entities;

namespace SWD.Data;

public partial class StockMarketDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public StockMarketDbContext()
    {
    }

    public StockMarketDbContext(DbContextOptions<StockMarketDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(local);Database=StockMarketDB;User Id=sa;Password=Abc@1234;Trusted_Connection=True;TrustServerCertificate=True");
       }
    }


    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Market> Markets { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationUser> NotificationUsers { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<StockHistory> StockHistories { get; set; }

    public virtual DbSet<StockInSession> StockInSessions { get; set; }
    
    public virtual DbSet<DeviceToken> DeviceTokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WatchList> WatchLists { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole<int> { Id = 2, Name = "MEMBERS", NormalizedName = "MEMBERS" },
            new IdentityRole<int> { Id = 3, Name = "MARKETANALIZER", NormalizedName = "MARKETANALIZER" },
            new IdentityRole<int> { Id = 4, Name = "USER", NormalizedName = "USER" }
        );
        modelBuilder.Entity<NotificationUser>()
       .HasKey(nu => new { nu.NotificationId, nu.UserId });

        // Define relationships
        modelBuilder.Entity<NotificationUser>()
            .HasOne(nu => nu.Notification)
            .WithMany(n => n.NotificationUsers)
            .HasForeignKey(nu => nu.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NotificationUser>()
            .HasOne(nu => nu.User)
            .WithMany(u => u.NotificationUsers)
            .HasForeignKey(nu => nu.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }

}
