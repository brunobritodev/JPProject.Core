using JPProject.Admin.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Threading.Tasks;

namespace JPProject.Admin.Infra.Data.MigrationHelper
{
    public static class DbMigrationHelpers
    {
        /// <summary>
        /// Generate migrations before running this method, you can use command bellow:
        /// Nuget package manager: Add-Migration DbInit -context ApplicationIdentityContext -output Data/Migrations
        /// Dotnet CLI: dotnet ef migrations add DbInit -c ApplicationIdentityContext -o Data/Migrations
        /// </summary>
        public static async Task EnsureSeedData(IServiceScope serviceScope)
        {
            var services = serviceScope.ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var id4Context = scope.ServiceProvider.GetRequiredService<IdentityServerContext>();
            var storeDb = scope.ServiceProvider.GetRequiredService<EventStoreContext>();

            await WaitForDb(id4Context);
            await storeDb.Database.GetPendingMigrationsAsync();
            await storeDb.Database.MigrateAsync();

            await CheckIs4Database(id4Context);

        }

        private static async Task CheckIs4Database(DbContext context)
        {
            var conn = context.Database.GetDbConnection();
            if (conn.State.Equals(ConnectionState.Closed)) await conn.OpenAsync();
            await using var command = conn.CreateCommand();

            command.CommandText = @"
                                    SELECT 1 FROM sys.tables AS T
                                        INNER JOIN sys.schemas AS S ON T.schema_id = S.schema_id
                                    WHERE S.Name = 'SchemaName' AND T.Name = 'Clients'";
            var exists = await command.ExecuteScalarAsync() != null;
        }


        private static async Task WaitForDb(DbContext context)
        {
            var maxAttemps = 3;
            var delay = 5000;

            var healthChecker = new DbHealthChecker();
            for (int i = 0; i < maxAttemps; i++)
            {
                var canConnect = await healthChecker.TestConnection(context);
                if (canConnect)
                {
                    return;
                }
                await Task.Delay(delay);
            }

            // after a few attemps we give up
            throw new Exception("Error wating database");

        }
    }
}
