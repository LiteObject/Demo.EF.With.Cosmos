using Demo.EF.With.Cosmos.Data;
using Demo.EF.With.Cosmos.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Demo.EF.With.Cosmos.UnstructuredData
{
    public static class Sample
    {
        public static async Task Run()
        {
            Console.WriteLine();
            Console.WriteLine("Unstructured data:");
            Console.WriteLine();

            #region Unmapped
            using (OrderCosmosDbContext context = new())
            {
                _ = await context.Database.EnsureDeletedAsync();
                _ = await context.Database.EnsureCreatedAsync();

                Order order = new()
                {
                    Id = 1,
                    ShippingAddress = new StreetAddress { City = "London", Street = "221 B Baker St" },
                    PartitionKey = "1"
                };

                _ = context.Add(order);

                _ = await context.SaveChangesAsync();
            }

            using (OrderCosmosDbContext context = new())
            {
                Order order = await context.Orders.FirstAsync();
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Order> orderEntry = context.Entry(order);

                Microsoft.EntityFrameworkCore.ChangeTracking.PropertyEntry<Order, JObject> jsonProperty = orderEntry.Property<JObject>("__jObject");
                jsonProperty.CurrentValue["BillingAddress"] = "Clarence House";

                orderEntry.State = EntityState.Modified;

                _ = await context.SaveChangesAsync();
            }

            using (OrderCosmosDbContext context = new())
            {
                Order order = await context.Orders.FirstAsync();
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Order> orderEntry = context.Entry(order);
                Microsoft.EntityFrameworkCore.ChangeTracking.PropertyEntry<Order, JObject> jsonProperty = orderEntry.Property<JObject>("__jObject");

                Console.WriteLine($"First order will be billed to: {jsonProperty.CurrentValue["BillingAddress"]}");
            }
            #endregion

            #region CosmosClient
            using (OrderCosmosDbContext context = new())
            {
                CosmosClient cosmosClient = context.Database.GetCosmosClient();
                Database database = cosmosClient.GetDatabase("OrdersDB");
                Container container = database.GetContainer("Orders");

                FeedIterator<JObject> resultSet = container.GetItemQueryIterator<JObject>(new QueryDefinition("select * from o"));
                JObject order = (await resultSet.ReadNextAsync()).First();

                Console.WriteLine($"First order JSON: {order}");

                _ = order.Remove("TrackingNumber");

                _ = await container.ReplaceItemAsync(order, order["id"].ToString());
            }
            #endregion

            #region MissingProperties
            using (OrderCosmosDbContext context = new())
            {
                List<Order> orders = await context.Orders.ToListAsync();
                List<Order> sortedOrders = await context.Orders.OrderBy(o => o.TrackingNumber).ToListAsync();

                Console.WriteLine($"Number of orders: {orders.Count}");
                Console.WriteLine($"Number of sorted orders: {sortedOrders.Count}");
            }
            #endregion
        }
    }
}
