using Demo.EF.With.Cosmos.Data;
using Demo.EF.With.Cosmos.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.EF.With.Cosmos
{
    /// <summary>
    /// Original Article:
    /// https://github.com/dotnet/EntityFramework.Docs/tree/main/samples/core/Cosmos
    /// </summary>
    internal class Program
    {
        static async Task Main()
        {
            //await Sample.Run();
            //await UnstructuredData.Sample.Run();

            using OrderCosmosDbContext context = new();
            //await context.Database.EnsureDeletedAsync();
            //await context.Database.EnsureCreatedAsync();

            DbSet<Order> orders = context.Orders;
            Console.WriteLine(orders.ToQueryString());
            (await orders.ToListAsync()).ForEach(o => Console.WriteLine(o.ShippingAddress.City));
        }

        private static async Task SqlDemoAsync()
        {
            using OrderSqlDbContext sqlDbContext = new();
            _ = await sqlDbContext.Database.EnsureDeletedAsync();
            _ = await sqlDbContext.Database.EnsureCreatedAsync();

            await sqlDbContext.Orders.AddRangeAsync(
                new Order { Id = 1, PartitionKey = "", ShippingAddress = new StreetAddress { Street = "123 Preston Road", City = "Dallas" }, TrackingNumber = 123456 },
                new Order { Id = 2, PartitionKey = "", ShippingAddress = new StreetAddress { Street = "456 Coit Road", City = "Plano" }, TrackingNumber = 234567 }
                );

            _ = await sqlDbContext.SaveChangesAsync();

            IQueryable<Order> orders = sqlDbContext.Orders.Where(o => o.Id == 1).TagWith("The demo app");
            Console.WriteLine("Query:" + orders.ToQueryString());
            // orders.ToList().ForEach(o => Console.WriteLine(o.TrackingNumber));

            Console.WriteLine("\n\nDone...");
            _ = Console.Read();
        }
    }
}