using JPProject.Sso.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JPProject.Sso.Data.Sqlite.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentitySqlite(this IServiceCollection services, string connectionString)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;

            services.AddEntityFrameworkSqlite().AddDbContext<ApplicationIdentityContext>(options => options.UseSqlite(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            return services;
        }
    }
}
