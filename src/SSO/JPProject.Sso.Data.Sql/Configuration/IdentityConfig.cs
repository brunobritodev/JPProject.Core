using JPProject.Sso.Infra.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace JPProject.Sso.SqlServer.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection WithSqlServer(this IIdentityServerBuilder builder, string connectionString)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;
            builder.Services.AddEntityFrameworkSqlServer().AddSsoContext(opt => opt.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
            builder.AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = opt => opt.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = opt => opt.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 15; // frequency in seconds to cleanup stale grants. 15 is useful during debugging
                });

            return builder.Services;
        }

        public static IServiceCollection WithSqlServer(this IIdentityServerBuilder builder, Action<DbContextOptionsBuilder> optionsAction)
        {
            builder.Services.AddEntityFrameworkSqlServer().AddSsoContext(optionsAction);
            builder.AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = optionsAction;
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = optionsAction;

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 15; // frequency in seconds to cleanup stale grants. 15 is useful during debugging
                });

            return builder.Services;
        }
    }
}
