using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Demo.EF.With.Cosmos.Data
{
    public class OptionsContext : DbContext
    {
        #region Configuration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseCosmos(
                        "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                        databaseName: "OptionsDB",
                        options =>
                        {
                            _ = options.ConnectionMode(ConnectionMode.Gateway);
                            _ = options.WebProxy(new WebProxy());
                            _ = options.LimitToEndpoint();
                            _ = options.Region(Regions.AustraliaCentral);
                            _ = options.GatewayModeMaxConnectionLimit(32);
                            _ = options.MaxRequestsPerTcpConnection(8);
                            _ = options.MaxTcpConnectionsPerEndpoint(16);
                            _ = options.IdleTcpConnectionTimeout(TimeSpan.FromMinutes(1));
                            _ = options.OpenTcpConnectionTimeout(TimeSpan.FromMinutes(1));
                            _ = options.RequestTimeout(TimeSpan.FromMinutes(1));
                        });
        }
        #endregion
    }
}
