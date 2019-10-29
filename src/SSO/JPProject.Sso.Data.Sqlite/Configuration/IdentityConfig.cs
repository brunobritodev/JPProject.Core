using System;
using System.Reflection;
using JPProject.Sso.Infra.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.EntityFrameworkCore.Sqlite.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentitySqlite(this IServiceCollection services, string connectionString)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;

            services.AddEntityFrameworkSqlite().AddSsoContext(options => options.UseSqlite(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            return services;
        }

        public static IServiceCollection AddIdentitySqlite(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.AddEntityFrameworkSqlite().AddSsoContext(optionsAction);

            return services;
        }
    }
}
