using JPProject.Domain.Core.Events;
using JPProject.EntityFrameworkCore.Context;
using JPProject.EntityFrameworkCore.MigrationHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace JPProject.EntityFrameworkCore.Configuration
{
    public static class DbMigrationHelpers
    {
        public static async Task CheckDatabases(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var storeDb = scope.ServiceProvider.GetRequiredService<EventStoreContext>();

            if (storeDb.Database.IsInMemory())
                return;

            await DbHealthChecker.TestConnection(storeDb);
            await ConfigureEventStoreContext(storeDb);
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
    }
}
