using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JPProject.Sso.Application.Configuration.DependencyInjection
{
#pragma warning disable S101 // Types should be named in PascalCase
    public static class SSSOBootstrapper
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public static IServiceCollection ConfigureSso<T>(this IServiceCollection services) where T : class, ISystemUser
        {
            // Domain Bus (Mediator)
            services.TryAddScoped<IMediatorHandler, InMemoryBus>();

            services.TryAddScoped<ISystemUser, T>();

            // Application
            ApplicationBootStrapper.RegisterServices(services);

            // Domain - Events
            DomainEventsBootStrapper.RegisterServices(services);

            // Domain - Commands
            DomainCommandsBootStrapper.RegisterServices(services);

            // Infra - Data
            RepositoryBootStrapper.RegisterServices(services);

            // Infra - Identity Services
            IdentityBootStrapper.RegisterServices(services);


            return services;
        }
    }
}
