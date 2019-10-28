using JPProject.Sso.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JPProject.Sso.Data.MySql.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityMySql(this IServiceCollection services, string connectionString)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;

            services.AddEntityFrameworkMySql()
                .AddDbContext<ApplicationIdentityContext>(options => options.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
            return services;
        }
    }
}
