using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SWD.Data.Entities;

public partial class StockMarketDbContext : DbContext
{
    public StockMarketDbContext()
    {
    }

    public StockMarketDbContext(DbContextOptions<StockMarketDbContext> options)
        : base(options)
    {
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

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WatchList> WatchLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);uid=sa;pwd=12345;database=StockMarketDB;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Company__2D971C4C42D7F6B6");

            entity.ToTable("Company");

            entity.HasIndex(e => e.CompanyName, "UQ__Company__9BCE05DC38A5409A").IsUnique();

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Ceo)
                .HasMaxLength(255)
                .HasColumnName("CEO");
            entity.Property(e => e.CompanyName).HasMaxLength(255);
        });

        modelBuilder.Entity<Market>(entity =>
        {
            entity.HasKey(e => e.MarketId).HasName("PK__Market__74B1864F8EB7340A");

            entity.ToTable("Market");

            entity.HasIndex(e => e.MarketName, "UQ__Market__F1A8D89C3D574E5D").IsUnique();

            entity.Property(e => e.MarketId).HasColumnName("MarketID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.EstablishedDate).HasColumnType("datetime");
            entity.Property(e => e.MarketName).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Website).HasMaxLength(255);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDD3DFBD529A");

            entity.Property(e => e.NewsId).HasColumnName("NewsID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LastEdited).HasColumnType("datetime");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Url).HasColumnName("URL");

            entity.HasOne(d => d.Staff).WithMany(p => p.News)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__News__StaffID__49C3F6B7");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32BD5466A6");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Navigation).HasMaxLength(100);
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Staff).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__Notificat__Staff__46E78A0C");
        });

        modelBuilder.Entity<NotificationUser>(entity =>
        {
            entity.HasKey(e => new { e.NotificationId, e.UserId }).HasName("PK__Notifica__F1B7A2F8F19EC35F");

            entity.ToTable("Notification_User");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Notification).WithMany(p => p.NotificationUsers)
                .HasForeignKey(d => d.NotificationId)
                .HasConstraintName("FK__Notificat__Notif__4CA06362");

            entity.HasOne(d => d.User).WithMany(p => p.NotificationUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notificat__UserI__4D94879B");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__Session__C9F492705F2BA317");

            entity.ToTable("Session");

            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.SessionType).HasMaxLength(50);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("PK__Stock__2C83A9E20B25A913");

            entity.ToTable("Stock");

            entity.HasIndex(e => e.StockSymbol, "UQ__Stock__E2FE0993A87D7EC6").IsUnique();

            entity.Property(e => e.StockId).HasColumnName("StockID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.ListedDate).HasColumnType("datetime");
            entity.Property(e => e.MarketId).HasColumnName("MarketID");
            entity.Property(e => e.StockSymbol).HasMaxLength(50);

            entity.HasOne(d => d.Company).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__Stock__CompanyID__31EC6D26");

            entity.HasOne(d => d.Market).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.MarketId)
                .HasConstraintName("FK__Stock__MarketID__32E0915F");

            entity.HasMany(d => d.WatchLists).WithMany(p => p.Stocks)
                .UsingEntity<Dictionary<string, object>>(
                    "StockWatchList",
                    r => r.HasOne<WatchList>().WithMany()
                        .HasForeignKey("WatchListId")
                        .HasConstraintName("FK__Stock_Wat__Watch__440B1D61"),
                    l => l.HasOne<Stock>().WithMany()
                        .HasForeignKey("StockId")
                        .HasConstraintName("FK__Stock_Wat__Stock__4316F928"),
                    j =>
                    {
                        j.HasKey("StockId", "WatchListId").HasName("PK__Stock_Wa__D54773D72CE3B494");
                        j.ToTable("Stock_WatchList");
                        j.IndexerProperty<int>("StockId").HasColumnName("StockID");
                        j.IndexerProperty<int>("WatchListId").HasColumnName("WatchListID");
                    });
        });

        modelBuilder.Entity<StockHistory>(entity =>
        {
            entity.HasKey(e => e.StockHistoryId).HasName("PK__StockHis__A6CE86DB94E18393");

            entity.ToTable("StockHistory");

            entity.Property(e => e.StockHistoryId).HasColumnName("StockHistoryID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Lsreasonchange)
                .HasMaxLength(255)
                .HasColumnName("LSReasonchange");
            entity.Property(e => e.Osreasonchange)
                .HasMaxLength(255)
                .HasColumnName("OSReasonchange");
            entity.Property(e => e.Rcreasonchange)
                .HasMaxLength(255)
                .HasColumnName("RCReasonchange");
            entity.Property(e => e.RegisteredCapital).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StockSymbol).HasMaxLength(50);
        });

        modelBuilder.Entity<StockInSession>(entity =>
        {
            entity.HasKey(e => e.StockInSessionId).HasName("PK__StockInS__A5EE923F42545DB4");

            entity.ToTable("StockInSession");

            entity.Property(e => e.StockInSessionId).HasColumnName("StockInSessionID");
            entity.Property(e => e.ClosePrice).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.HighPrice).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.LowPrice).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.OpenPrice).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.StockId).HasColumnName("StockID");

            entity.HasOne(d => d.Session).WithMany(p => p.StockInSessions)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__StockInSe__Sessi__38996AB5");

            entity.HasOne(d => d.Stock).WithMany(p => p.StockInSessions)
                .HasForeignKey(d => d.StockId)
                .HasConstraintName("FK__StockInSe__Stock__37A5467C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC828E84B3");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ__User__536C85E4DFC26959").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534B8C81ACA").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.LastEdited)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SubscriptionStatus).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        modelBuilder.Entity<WatchList>(entity =>
        {
            entity.HasKey(e => e.WatchListId).HasName("PK__WatchLis__9C4DA35E12711C68");

            entity.ToTable("WatchList");

            entity.Property(e => e.WatchListId).HasColumnName("WatchListID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Label).HasMaxLength(255);
            entity.Property(e => e.LastEdited)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.WatchLists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__WatchList__UserI__3E52440B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
