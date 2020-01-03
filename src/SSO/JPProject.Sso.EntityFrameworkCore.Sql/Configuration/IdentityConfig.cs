using JPProject.EntityFrameworkCore.Context;
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
        public static ISsoConfigurationBuilder AddEventStoreSqlServer(this ISsoConfigurationBuilder services, string connectionString)
        {
            var migrationsAssembly = typeof(IdentityConfig).GetTypeInfo().Assembly.GetName().Name;

            services.Services.AddDbContext<EventStoreContext>(opt => opt.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            return services;
        }

        public static ISsoConfigurationBuilder WithSqlServer(this ISsoConfigurationBuilder builder, Action<DbContextOptionsBuilder> optionsAction)
        {
            builder.Services.AddEntityFrameworkSqlServer().AddSsoContext(optionsAction);

            return builder;
        }

        public static ISsoConfigurationBuilder AddEventStoreSqlite(this ISsoConfigurationBuilder services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.Services.AddDbContext<EventStoreContext>(optionsAction);

            return services;
        }
    }
}
