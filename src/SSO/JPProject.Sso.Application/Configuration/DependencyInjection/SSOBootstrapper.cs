using IdentityServer4.Services;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Application.CloudServices.Storage;
using JPProject.Sso.Application.Configuration;
using JPProject.Sso.Application.Configuration.DependencyInjection;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Domain.Interfaces;
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
        /// <typeparam name="TUserService">Provide a services for SSO manipulate users</typeparam>
        /// <typeparam name="TRoleService">Provide a service for SSO manipulate Roles</typeparam>
        /// <returns></returns>
        public static ISsoConfigurationBuilder ConfigureSso<THttpUser, TUserService, TRoleService>(this IServiceCollection services)
            where TUserService : class, IUserService
            where TRoleService : class, IRoleService
            where THttpUser : class, ISystemUser
        {
            services.TryAddScoped<IMediatorHandler, InMemoryBus>();
            services.TryAddScoped<ISystemUser, THttpUser>();
            services.AddScoped<IEventSink, IdentityServerEventStore>();
            services.AddScoped<IUserService, TUserService>();
            services.AddScoped<IRoleService, TRoleService>();
            services.AddScoped<IStorage, StorageService>();

            services
                .AddApplicationServices()
                .AddDomainEvents()
                .AddDomainCommands();

            return new SsoBuilder(services);
        }
    }
}
