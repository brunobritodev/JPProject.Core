using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace JPProject.Sso.Application.Configuration.DependencyInjection
{
#pragma warning disable S101 // Types should be named in PascalCase
    public static class SSOBootstrapper
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public static IServiceCollection ConfigureSso<THttpUser, TUser, TRole, TKey>(this IServiceCollection services)
            where THttpUser : class, ISystemUser
            where TUser : IdentityUser<TKey>, IDomainUser
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            services
                .BaseSsoConfiguration<THttpUser>()
                .AddIdentity<TUser, TRole>()
                .AddEntityFrameworkStores<ApplicationIdentityContext<TUser, TRole, TKey>>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection ConfigureSso<THttpUser>(this IServiceCollection services)
            where THttpUser : class, ISystemUser
        {
            services
                .BaseSsoConfiguration<THttpUser>()
                .AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationIdentityContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection ConfigureSso<THttpUser, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>(this IServiceCollection services)
            where THttpUser : class, ISystemUser
            where TUser : IdentityUser<TKey>
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
            where TUserClaim : IdentityUserClaim<TKey>
            where TUserRole : IdentityUserRole<TKey>
            where TUserLogin : IdentityUserLogin<TKey>
            where TRoleClaim : IdentityRoleClaim<TKey>
            where TUserToken : IdentityUserToken<TKey>
        {
            services
                .BaseSsoConfiguration<THttpUser>()
                .AddIdentity<TUser, TRole>()
                .AddEntityFrameworkStores<ApplicationIdentityContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>>()
                .AddDefaultTokenProviders();

            return services;
        }

        private static IServiceCollection BaseSsoConfiguration<T>(this IServiceCollection services)
            where T : class, ISystemUser
        {
            // Domain Bus (Mediator)
            services.TryAddScoped<IMediatorHandler, InMemoryBus>();
            services.TryAddScoped<ISystemUser, T>();


            services
                .AddApplicationServices()
                .AddDomainEvents()
                .AddDomainCommands()
                .AddStores()
                .AddIdentityServices();
            return services;
        }
    }
}
