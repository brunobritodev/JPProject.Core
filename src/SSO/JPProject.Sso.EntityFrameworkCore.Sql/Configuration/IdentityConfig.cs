using JPProject.EntityFrameworkCore.Configuration;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace JPProject.Sso.EntityFrameworkCore.SqlServer.Configuration
{
    public static class IdentityConfig
    {

        public static ISsoConfigurationBuilder WithSqlServer(this ISsoConfigurationBuilder builder, string connectionString)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;
            builder.Services.AddEntityFrameworkSqlServer().AddSsoContext(opt => opt.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            return builder;
        }
        public static ISsoConfigurationBuilder AddEventStoreSqlServer(this ISsoConfigurationBuilder services, string connectionString, EventStoreMigrationOptions options = null)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;

            services.Services.AddEventStoreContext(opt => opt.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)), options);

            return services;
        }

        public static ISsoConfigurationBuilder WithSqlServer(this ISsoConfigurationBuilder builder, Action<DbContextOptionsBuilder> optionsAction)
        {
            builder.Services.AddEntityFrameworkSqlServer().AddSsoContext(optionsAction);

            return builder;
        }

        public static ISsoConfigurationBuilder AddEventStoreSqlite(this ISsoConfigurationBuilder services, Action<DbContextOptionsBuilder> optionsAction, EventStoreMigrationOptions options = null)
        {
            services.Services.AddEventStoreContext(optionsAction, options);

            return services;
        }
    }
}
