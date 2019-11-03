using JPProject.EntityFrameworkCore.Configuration;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace JPProject.Sso.EntityFrameworkCore.MySql.Configuration
{
    public static class IdentityConfig
    {
        public static ISsoConfigurationBuilder WithMySql(this ISsoConfigurationBuilder services, string connectionString)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;

            services.Services.AddEntityFrameworkMySql().AddSsoContext(options => options.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
            return services;
        }

        public static ISsoConfigurationBuilder AddEventStoreMySql(this ISsoConfigurationBuilder services, string connectionString, EventStoreMigrationOptions options = null)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;

            services.Services.AddEventStoreContext(opt => opt.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)), options);

            return services;
        }
        public static ISsoConfigurationBuilder WithMySql(this ISsoConfigurationBuilder services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.Services.AddEntityFrameworkMySql().AddSsoContext(optionsAction);

            return services;
        }

    }
}
