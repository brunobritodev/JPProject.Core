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
                    services.WithMySql(connString);
                    break;
                case DatabaseType.SqlServer:
                    services.WithSqlServer(connString);
                    break;
                case DatabaseType.Postgre:
                    services.WithPostgreSql(connString);
                    break;
                case DatabaseType.Sqlite:
                    services.WithSqlite(connString);
                    break;
            }

            return services;
        }

    }
}
