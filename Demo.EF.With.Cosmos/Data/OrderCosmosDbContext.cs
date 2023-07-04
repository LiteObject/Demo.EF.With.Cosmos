using Demo.EF.With.Cosmos.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.EF.With.Cosmos.Data
{
    public class OrderCosmosDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Distributor> Distributors { get; set; }

        #region Configuration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseCosmos(
                        "https://localhost:8081",
                        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                        databaseName: "OrdersDb");
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region DefaultContainer
            _ = modelBuilder.HasDefaultContainer("Store");
            #endregion

            #region Container
            _ = modelBuilder.Entity<Order>()
                .ToContainer("Orders");
            #endregion

            #region NoDiscriminator
            _ = modelBuilder.Entity<Order>()
                .HasNoDiscriminator();
            #endregion

            #region PartitionKey
            _ = modelBuilder.Entity<Order>()
                .HasPartitionKey(o => o.PartitionKey);
            #endregion

            #region ETag
            _ = modelBuilder.Entity<Order>()
                .UseETagConcurrency();
            #endregion

            #region PropertyNames
            _ = modelBuilder.Entity<Order>().OwnsOne(
                o => o.ShippingAddress,
                sa =>
                {
                    _ = sa.ToJsonProperty("Address");
                    _ = sa.Property(p => p.Street).ToJsonProperty("ShipsToStreet");
                    _ = sa.Property(p => p.City).ToJsonProperty("ShipsToCity");
                });
            #endregion

            #region OwnsMany
            _ = modelBuilder.Entity<Distributor>().OwnsMany(p => p.ShippingCenters);
            #endregion

            #region ETagProperty
            _ = modelBuilder.Entity<Distributor>()
                .Property(d => d.ETag)
                .IsETagConcurrency();
            #endregion
        }
    }
}
