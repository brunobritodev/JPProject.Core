using IdentityServer4.EntityFramework.Entities;
using JPProject.Admin.Infra.Data.Context;
using JPProject.Domain.Core.Events;
using JPProject.Domain.Core.Exceptions;
using JPProject.EntityFrameworkCore.Context;
using JPProject.EntityFrameworkCore.MigrationHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace JPProject.Admin.Infra.Data.Configuration
{
    public static class DbMigrationHelpers
    {
        public static async Task CheckDatabases(IServiceProvider serviceProvider, JpDatabaseOptions options)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var id4Context = scope.ServiceProvider.GetRequiredService<JPProjectAdminUIContext>();
            var storeDb = scope.ServiceProvider.GetRequiredService<EventStoreContext>();

            if (id4Context.Database.IsInMemory() || storeDb.Database.IsInMemory())
                return;

            await DbHealthChecker.TestConnection(id4Context);
            await ValidateIs4Context(options, id4Context);

            await ConfigureEventStoreContext(storeDb);
        }

        private static async Task ValidateIs4Context(JpDatabaseOptions options, JPProjectAdminUIContext id4AdminUiContext)
        {
            var configurationDatabaseExist = await DbHealthChecker.CheckTableExists<Client>(id4AdminUiContext);
            var operationalDatabaseExist = await DbHealthChecker.CheckTableExists<PersistedGrant>(id4AdminUiContext);
            var isDatabaseExist = configurationDatabaseExist && operationalDatabaseExist;

            if (!isDatabaseExist && options.MustThrowExceptionIfDatabaseDontExist)
                throw new DatabaseNotFoundException("IdentityServer4 Database doesn't exist. Ensure it was created before.'");
        }

        private static async Task ConfigureEventStoreContext(EventStoreContext storeDb)
        {
            var storeDbExist = await DbHealthChecker.CheckTableExists<StoredEvent>(storeDb);
            if (!storeDbExist)
                await storeDb.Database.MigrateAsync();
        }


    }
}
