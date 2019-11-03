using JPProject.Domain.Core.ViewModels;
using JPProject.EntityFrameworkCore.Configuration;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.EntityFrameworkCore.MySql.Configuration;
using JPProject.Sso.EntityFrameworkCore.PostgreSQL.Configuration;
using JPProject.Sso.EntityFrameworkCore.Sqlite.Configuration;
using JPProject.Sso.EntityFrameworkCore.SqlServer.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.Database
{
    public static class DbSettingsConfig
    {
        public static ISsoConfigurationBuilder AddDatabase(this ISsoConfigurationBuilder services, DatabaseType database, string connString)
        {
            switch (database)
            {
                case DatabaseType.MySql:
                    services.WithMySql(connString).AddEventStoreMySql(connString, EventStoreMigrationOptions.Get().ShouldMigrate(false));
                    break;
                case DatabaseType.SqlServer:
                    services.WithSqlServer(connString).AddEventStoreSqlServer(connString, EventStoreMigrationOptions.Get().ShouldMigrate(false));
                    break;
                case DatabaseType.Postgre:
                    services.WithPostgreSql(connString).AddEventStorePostgreSql(connString, EventStoreMigrationOptions.Get().ShouldMigrate(false));
                    break;
                case DatabaseType.Sqlite:
                    services.WithSqlite(connString).AddEventStoreSqlite(connString, EventStoreMigrationOptions.Get().ShouldMigrate(false));
                    break;
            }

            return services;
        }

        public static IIdentityServerBuilder AddOAuth2Database(this IIdentityServerBuilder builder, DatabaseType database, string connString)
        {
            switch (database)
            {
                case DatabaseType.MySql:
                    builder.WithMySql(connString);
                    break;
                case DatabaseType.SqlServer:
                    builder.WithSqlServer(connString);
                    break;
                case DatabaseType.Postgre:
                    builder.WithPostgreSql(connString);
                    break;
                case DatabaseType.Sqlite:
                    builder.WithSqlite(connString);
                    break;
            }
            return builder;
        }
    }
}