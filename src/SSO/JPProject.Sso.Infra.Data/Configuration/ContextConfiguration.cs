using JPProject.EntityFrameworkCore.Interfaces;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.Infra.Data.Configuration
{
    public static class ContextConfiguration
    {
        public static ISsoConfigurationBuilder AddSsoContext<TContext>(this ISsoConfigurationBuilder services) where TContext : class, ISsoContext
        {
            services.Services.AddScoped<ISsoContext, TContext>();
            services.Services.AddScoped<IJpEntityFrameworkStore, TContext>();
            return services;
        }
    }
}
