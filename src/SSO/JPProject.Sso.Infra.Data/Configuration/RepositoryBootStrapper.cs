using JPProject.Domain.Core.Events;
using JPProject.Domain.Core.Interfaces;
using JPProject.EntityFrameworkCore.EventSourcing;
using JPProject.EntityFrameworkCore.Repository;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Data.Repository;
using JPProject.Sso.Infra.Data.UoW;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.Infra.Data.Configuration
{
    internal static class RepositoryBootStrapper
    {
        public static IServiceCollection AddStores(this IServiceCollection services)
        {
            services.AddScoped<ISsoUnitOfWork, UnitOfWork>();

            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
            services.AddScoped<IGlobalConfigurationSettingsRepository, GlobalConfigurationSettingsRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
