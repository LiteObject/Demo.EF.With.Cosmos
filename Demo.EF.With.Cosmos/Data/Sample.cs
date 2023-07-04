using Demo.EF.With.Cosmos.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.EF.With.Cosmos.Data
{
    public static class Sample
    {
        public static async Task Run()
        {
            Console.WriteLine();
            Console.WriteLine("Getting started with Cosmos:");
            Console.WriteLine();

            #region HelloCosmos
            using (OrderCosmosDbContext context = new())
            {
                _ = await context.Database.EnsureDeletedAsync();
                _ = await context.Database.EnsureCreatedAsync();

                _ = context.Add(
                    new Order
                    {
                        Id = 1,
                        ShippingAddress = new StreetAddress { City = "London", Street = "221 B Baker St" },
                        PartitionKey = "1"
                    });

                _ = await context.SaveChangesAsync();
            }

            using (OrderCosmosDbContext context = new())
            {
                Order order = await context.Orders.FirstAsync();
                Console.WriteLine($"First order will ship to: {order.ShippingAddress.Street}, {order.ShippingAddress.City}");
                Console.WriteLine();
            }
            #endregion

            #region PartitionKey
            using (OrderCosmosDbContext context = new())
            {
                _ = context.Add(
                    new Order
                    {
                        Id = 2,
                        ShippingAddress = new StreetAddress { City = "New York", Street = "11 Wall Street" },
                        PartitionKey = "2"
                    });

                _ = await context.SaveChangesAsync();
            }

            using (OrderCosmosDbContext context = new())
            {
                Order order = await context.Orders.WithPartitionKey("2").LastAsync();
                Console.WriteLine($"Last order will ship to: {order.ShippingAddress.Street}, {order.ShippingAddress.City}");
                Console.WriteLine();
            }
            #endregion

            #region OwnedCollection
            Distributor distributor = new()
            {
                Id = 1,
                ShippingCenters = new HashSet<StreetAddress>
                {
                    new StreetAddress { City = "Phoenix", Street = "500 S 48th Street" },
                    new StreetAddress { City = "Anaheim", Street = "5650 Dolly Ave" }
                }
            };

            using (OrderCosmosDbContext context = new())
            {
                _ = context.Add(distributor);

                _ = await context.SaveChangesAsync();
            }
            #endregion

            #region ImpliedProperties
            using (OrderCosmosDbContext context = new())
            {
                Distributor firstDistributor = await context.Distributors.FirstAsync();
                Console.WriteLine($"Number of shipping centers: {firstDistributor.ShippingCenters.Count}");

                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<StreetAddress> addressEntry = context.Entry(firstDistributor.ShippingCenters.First());
                IReadOnlyList<Microsoft.EntityFrameworkCore.Metadata.IProperty> addressPKProperties = addressEntry.Metadata.FindPrimaryKey().Properties;

                Console.WriteLine(
                    $"First shipping center PK: ({addressEntry.Property(addressPKProperties[0].Name).CurrentValue}, {addressEntry.Property(addressPKProperties[1].Name).CurrentValue})");
                Console.WriteLine();
            }
            #endregion

            #region Attach
            using (OrderCosmosDbContext context = new())
            {
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Distributor> distributorEntry = context.Add(distributor);
                distributorEntry.State = EntityState.Unchanged;

                _ = distributor.ShippingCenters.Remove(distributor.ShippingCenters.Last());

                _ = await context.SaveChangesAsync();
            }

            using (OrderCosmosDbContext context = new())
            {
                Distributor firstDistributor = await context.Distributors.FirstAsync();
                Console.WriteLine($"Number of shipping centers is now: {firstDistributor.ShippingCenters.Count}");

                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Distributor> distributorEntry = context.Entry(firstDistributor);
                Microsoft.EntityFrameworkCore.ChangeTracking.PropertyEntry<Distributor, string> idProperty = distributorEntry.Property<string>("__id");
                Console.WriteLine($"The distributor 'id' is: {idProperty.CurrentValue}");
            }
            #endregion
        }
    }
}
