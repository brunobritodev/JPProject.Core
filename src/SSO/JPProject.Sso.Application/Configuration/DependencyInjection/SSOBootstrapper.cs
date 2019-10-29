using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Infra.CrossCutting.Identity.Models.Identity;
using JPProject.Sso.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JPProject.Sso.Application.Configuration.DependencyInjection
{
#pragma warning disable S101 // Types should be named in PascalCase
    public static class SSOBootstrapper
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public static IServiceCollection ConfigureSso<THttpUser>(this IServiceCollection services)
            where THttpUser : class, ISystemUser
        {
            services
                .BaseSsoConfiguration<THttpUser>()
                .AddIdentity<UserIdentity, UserIdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityContext>()
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
