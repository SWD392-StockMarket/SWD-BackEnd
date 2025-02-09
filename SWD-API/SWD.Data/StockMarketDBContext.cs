using Microsoft.EntityFrameworkCore;
using SWD.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data
{
    public class StockMarketDbContext : DbContext
    {
        public StockMarketDbContext(DbContextOptions<StockMarketDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Market> Markets { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<StockInSession> StockInSessions { get; set; }
        public DbSet<StockHistory> StockHistories { get; set; }
        public DbSet<WatchList> WatchLists { get; set; }
        public DbSet<StockWatchList> StockWatchLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StockWatchList>()
                .HasKey(sw => new { sw.StockID, sw.WatchListID });
        }
    }
}
