using JPProject.EntityFrameworkCore.Configuration;
using JPProject.Sso.Infra.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace JPProject.Sso.EntityFrameworkCore.MySql.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection WithMySql(this IServiceCollection services, string connectionString)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;

            services.AddEntityFrameworkMySql().AddSsoContext(options => options.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
            return services;
        }

        public static IServiceCollection AddEventStoreMySql(this IServiceCollection services, string connectionString)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;

            services.AddEventStoreContext(options => options.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
            return services;
        }
        public static IServiceCollection WithMySql(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.AddEntityFrameworkMySql().AddSsoContext(optionsAction);

            return services;
        }
    }
}
