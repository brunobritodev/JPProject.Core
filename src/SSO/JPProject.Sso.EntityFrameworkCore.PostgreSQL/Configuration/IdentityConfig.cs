using JPProject.EntityFrameworkCore.Configuration;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace JPProject.Sso.EntityFrameworkCore.PostgreSQL.Configuration
{
    public static class IdentityConfig
    {
        public static ISsoConfigurationBuilder WithPostgreSql<T>(this ISsoConfigurationBuilder services, string connectionString)
        {
            var migrationsAssembly = typeof(T).GetTypeInfo().Assembly.GetName().Name;

            services.Services.AddEntityFrameworkNpgsql().AddSsoContext(options => options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            return services;
        }
        public static ISsoConfigurationBuilder AddEventStorePostgreSql<T>(this ISsoConfigurationBuilder services, string connectionString, EventStoreMigrationOptions options = null)
        {
            var migrationsAssembly = typeof(T).GetTypeInfo().Assembly.GetName().Name;

            services.Services.AddEventStoreContext(options => options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)), options);

            return services;
        }

        public static ISsoConfigurationBuilder WithPostgreSql(this ISsoConfigurationBuilder services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.Services.AddEntityFrameworkNpgsql().AddSsoContext(optionsAction);

            return services;
        }
    }
}
