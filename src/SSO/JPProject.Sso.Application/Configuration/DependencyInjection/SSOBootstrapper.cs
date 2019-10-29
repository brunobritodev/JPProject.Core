using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Infra.Data.Context;
using JPProject.Sso.Infra.Identity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JPProject.Sso.Application.Configuration.DependencyInjection
{
#pragma warning disable S101 // Types should be named in PascalCase
    public static class SSOBootstrapper
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public static IIdentityServerBuilder ConfigureSso<THttpUser>(this IServiceCollection services)
            where THttpUser : class, ISystemUser
        {

            services
                .BaseSsoConfiguration<THttpUser>()
                .AddIdentity<UserIdentity, UserIdentityRole>(AccountOptions.NistAccountOptions())
                .AddEntityFrameworkStores<ApplicationIdentityContext>()
                .AddDefaultTokenProviders();

            var is4Builder = services.AddIdentityServer(
                    options =>
                    {
                        options.Events.RaiseErrorEvents = true;
                        options.Events.RaiseInformationEvents = true;
                        options.Events.RaiseFailureEvents = true;
                        options.Events.RaiseSuccessEvents = true;
                    })
                .AddAspNetIdentity<UserIdentity>();

            return is4Builder;
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
