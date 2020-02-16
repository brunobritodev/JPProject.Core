using JPProject.EntityFrameworkCore.Interfaces;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.Infra.Data.Configuration
{
    public static class ContextConfiguration
    {
        public static ISsoConfigurationBuilder AddSsoContext<TContext, TEventStore>(this ISsoConfigurationBuilder services)
            where TContext : class, ISsoContext
            where TEventStore : class, IEventStoreContext

        {
            services.Services.AddScoped<IEventStoreContext, TEventStore>();
            services.Services.AddScoped<ISsoContext, TContext>();
            services.Services.AddScoped<IJpEntityFrameworkStore>(x => x.GetRequiredService<TContext>());
            services.Services.AddStores();
            return services;
        }
        public static ISsoConfigurationBuilder AddSsoContext<TContext>(this ISsoConfigurationBuilder services)
            where TContext : class, ISsoContext, IEventStoreContext

        {
            services.Services.AddScoped<ISsoContext, TContext>();
            services.Services.AddScoped<IEventStoreContext>(s => s.GetService<TContext>());
            services.Services.AddScoped<IJpEntityFrameworkStore>(x => x.GetRequiredService<TContext>());
            services.Services.AddStores();
            return services;
        }
    }
}
