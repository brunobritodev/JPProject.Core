using IdentityServer4.Services;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Configuration;
using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Application.CloudServices.Storage;
using JPProject.Sso.Application.Configuration;
using JPProject.Sso.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
#pragma warning disable S101 // Types should be named in PascalCase
    public static class SSOBootstrapper
#pragma warning restore S101 // Types should be named in PascalCase
    {
        /// <summary>
        /// Configure SSO Services
        /// </summary>
        /// <typeparam name="THttpUser">Implementation of ISystemUser</typeparam>
        /// <returns></returns>
        public static IJpProjectConfigurationBuilder ConfigureSso<THttpUser>(this IServiceCollection services)
            where THttpUser : class, ISystemUser
        {
            services.TryAddScoped<IMediatorHandler, InMemoryBus>();
            services.TryAddScoped<ISystemUser, THttpUser>();
            services.AddScoped<IEventSink, IdentityServerEventStore>();
            services.AddScoped<IStorage, StorageService>();

            services
                .AddApplicationServices()
                .AddDomainEvents()
                .AddDomainCommands();

            return new JpProjectBuilder(services);
        }

        /// <summary>   
        /// Configure SSO Services
        /// </summary>
        /// <typeparam name="THttpUser">Implementation of ISystemUser</typeparam>
        /// <returns></returns>
        public static IJpProjectConfigurationBuilder ConfigureSso<THttpUser>(this IJpProjectConfigurationBuilder builder)
            where THttpUser : class, ISystemUser
        {
            builder.Services.TryAddScoped<IMediatorHandler, InMemoryBus>();
            builder.Services.TryAddScoped<ISystemUser, THttpUser>();
            builder.Services.AddScoped<IEventSink, IdentityServerEventStore>();
            builder.Services.AddScoped<IStorage, StorageService>();
            builder.Services
                .AddApplicationServices()
                .AddDomainEvents()
                .AddDomainCommands();

            return new JpProjectBuilder(builder.Services);
        }
    }
}
