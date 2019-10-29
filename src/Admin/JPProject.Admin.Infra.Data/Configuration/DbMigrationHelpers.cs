using System;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using JPProject.Admin.Infra.Data.Context;
using JPProject.Domain.Core.Events;
using JPProject.Domain.Core.Exceptions;
using JPProject.EntityFrameworkCore.Context;
using JPProject.EntityFrameworkCore.MigrationHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Admin.Infra.Data.Configuration
{
    public static class DbMigrationHelpers
    {
        public static async Task CheckDatabases(IServiceProvider serviceProvider, JpDatabaseOptions options)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var id4Context = scope.ServiceProvider.GetRequiredService<JpProjectContext>();
            var storeDb = scope.ServiceProvider.GetRequiredService<EventStoreContext>();

            if (id4Context.Database.IsInMemory() || storeDb.Database.IsInMemory())
                return;

            await WaitForDb(id4Context);
            await ValidateIs4Context(options, id4Context);

            await ConfigureEventStoreContext(storeDb);
        }

        private static async Task ValidateIs4Context(JpDatabaseOptions options, JpProjectContext id4Context)
        {
            var configurationDatabaseExist = await CheckTableExists<Client>(id4Context);
            var operationalDatabaseExist = await CheckTableExists<PersistedGrant>(id4Context);
            var isDatabaseExist = configurationDatabaseExist && operationalDatabaseExist;

            if (!isDatabaseExist && options.MustThrowExceptionIfDatabaseDontExist)
                throw new DatabaseNotFoundException("IdentityServer4 Database doesn't exist. Ensure it was created before.'");
        }

        private static async Task ConfigureEventStoreContext(EventStoreContext storeDb)
        {
            var storeDbExist = await CheckTableExists<StoredEvent>(storeDb);
            if (!storeDbExist)
                await storeDb.Database.MigrateAsync();
        }

        /// <summary>
        /// Check if data table is exist in application
        /// </summary>
        /// <typeparam name="T">Class of data table to check</typeparam>
        /// <param name="db">DB Object</param>
        private static async Task<bool> CheckTableExists<T>(DbContext db) where T : class
        {
            try
            {
                await db.Set<T>().AnyAsync();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }


        private static async Task WaitForDb(DbContext context)
        {
            var maxAttemps = 3;
            var delay = 5000;

            var healthChecker = new DbHealthChecker();
            for (int i = 0; i < maxAttemps; i++)
            {
                var canConnect = healthChecker.TestConnection(context);
                if (canConnect)
                {
                    return;
                }
                await Task.Delay(delay);
            }

            // after a few attemps we give up
            throw new DatabaseNotFoundException("Error wating database. Check ConnectionString and ensure database exist");

        }
    }
}
