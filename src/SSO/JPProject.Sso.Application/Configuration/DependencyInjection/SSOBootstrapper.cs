using IdentityServer4.Services;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Application.Configuration;
using JPProject.Sso.Application.Configuration.DependencyInjection;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Data.Context;
using JPProject.Sso.Infra.Identity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
#pragma warning disable S101 // Types should be named in PascalCase
    public static class SSOBootstrapper
#pragma warning restore S101 // Types should be named in PascalCase
    {
        /// <summary>
        /// Configure User Identity - ASP.NET Identity
        /// </summary>
        /// <typeparam name="THttpUser">Implementation of ISystemUser</typeparam>
        /// <returns></returns>
        public static ISsoConfigurationBuilder ConfigureUserIdentity<THttpUser>(this IServiceCollection builder)
            where THttpUser : class, ISystemUser
        {
            var identityBuilder =
                builder
                    .BaseSsoConfiguration<THttpUser>()
                    .AddIdentity<UserIdentity, IdentityRole>(AccountOptions.NistAccountOptions())
                    .AddEntityFrameworkStores<ApplicationSsoContext>()
                    .AddDefaultTokenProviders();
            return new SsoBuilder(builder, identityBuilder);
        }

        public static IIdentityServerBuilder ConfigureIdentityServer(this ISsoConfigurationBuilder builder)
        {
            var is4Builder = builder.Services.AddIdentityServer(
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


        private static IServiceCollection BaseSsoConfiguration<THttpUser>(this IServiceCollection services)
            where THttpUser : class, ISystemUser
        {
            // Domain Bus (Mediator)
            services.TryAddScoped<IMediatorHandler, InMemoryBus>();
            services.TryAddScoped<ISystemUser, THttpUser>();
            services.AddScoped<IEventSink, IdentityServerEventStore>();
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
