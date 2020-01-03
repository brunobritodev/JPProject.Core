using JPProject.Admin.Domain.Interfaces;
using JPProject.Domain.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Admin.Database
{
    public static class DbSettingsConfig
    {
        public static IJpProjectAdminBuilder AddDatabase(this IJpProjectAdminBuilder services, DatabaseType database, string connString)
        {
            switch (database)
            {
                case DatabaseType.MySql:
                    services.WithMySql(connString).AddEventStoreMySql(connString);
                    break;
                case DatabaseType.SqlServer:
                    services.WithSqlServer(connString).AddEventStoreSqlServer(connString);
                    break;
                case DatabaseType.Postgre:
                    services.WithPostgreSql(connString).AddEventStorePostgreSql(connString);
                    break;
                case DatabaseType.Sqlite:
                    services.WithSqlite(connString).AddEventStoreSqlite(connString);
                    break;
            }

            return services;
        }

    }
}
