using Demo.EF.With.Cosmos.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.EF.With.Cosmos.Data
{
    public class OrderSqlDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public string DbPath { get; init; }

        public OrderSqlDbContext()
        {
            Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
            string path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "OrderDb");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlite($"Data Source={DbPath}");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<Order>().OwnsOne(p => p.ShippingAddress);
            base.OnModelCreating(modelBuilder);
        }
    }
}
