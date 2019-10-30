using System;
using System.Reflection;
using JPProject.EntityFrameworkCore.Configuration;
using JPProject.Sso.Infra.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.EntityFrameworkCore.PostgreSQL.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection WithPostgreSql(this IServiceCollection services, string connectionString)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;

            services.AddEntityFrameworkNpgsql().AddSsoContext(options => options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            return services;
        }
        public static IServiceCollection AddEventStorePostgreSql(this IServiceCollection services, string connectionString)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;

            services.AddEventStoreContext(options => options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            return services;
        }

        public static IServiceCollection WithPostgreSql(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.AddEntityFrameworkNpgsql().AddSsoContext(optionsAction);

            return services;
        }
    }
}
