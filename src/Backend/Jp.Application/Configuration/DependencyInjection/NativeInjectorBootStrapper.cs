using Jp.Application.Bus;
using Jp.Application.Configuration;
using Jp.Domain.Core.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdminUiBootstrapper
    {
        public static void ConfigureJpAdmin(this IServiceCollection services)
        {
            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // Application
            ApplicationBootStrapper.RegisterServices(services);

            // Domain - Events
            DomainEventsBootStrapper.RegisterServices(services);

            // Domain - Commands
            DomainCommandsBootStrapper.RegisterServices(services);

            // Infra - Data
            RepositoryBootStrapper.RegisterServices(services);
        }
    }
}