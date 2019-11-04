using JPProject.EntityFrameworkCore.Configuration;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace JPProject.Sso.EntityFrameworkCore.Sqlite.Configuration
{
    public static class IdentityConfig
    {
        public static ISsoConfigurationBuilder WithSqlite(this ISsoConfigurationBuilder services, string connectionString)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;

            services.Services.AddEntityFrameworkSqlite().AddSsoContext(options => options.UseSqlite(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            return services;
        }
        public static ISsoConfigurationBuilder WithSqlite(this ISsoConfigurationBuilder services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.Services.AddEntityFrameworkSqlite().AddSsoContext(optionsAction);

            return services;
        }

        public static ISsoConfigurationBuilder AddEventStoreSqlite(this ISsoConfigurationBuilder services, string connectionString, EventStoreMigrationOptions options = null)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;

            services.Services.AddEventStoreContext(opt => opt.UseSqlite(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)), options);

            return services;
        }

    }
}
