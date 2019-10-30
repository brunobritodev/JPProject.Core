using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace JPProject.Sso.EntityFrameworkCore.MySql.Configuration
{
    public static class IdentityServerConfig
    {
        public static IIdentityServerBuilder WithMySql<T>(
            this IIdentityServerBuilder builder,
            string connectionString)
        {
            var migrationsAssembly = typeof(T).GetTypeInfo().Assembly.GetName().Name;

            // this adds the config data from DB (clients, resources)
            builder.AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseMySql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    //options.TokenCleanupInterval = 15; // frequency in seconds to cleanup stale grants. 15 is useful during debugging
                });


            return builder;
        }

        public static IServiceCollection WithMySql(this IIdentityServerBuilder builder, Action<DbContextOptionsBuilder> optionsAction)
        {
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