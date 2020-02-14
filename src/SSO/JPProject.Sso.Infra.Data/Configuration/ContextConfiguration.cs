using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JPProject.Sso.Infra.Data.Configuration
{
    public static class ContextConfiguration
    {
        public static ISsoConfigurationBuilder AddSsoContext(this ISsoConfigurationBuilder services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.Services.AddDbContext<ApplicationSsoContext>(optionsAction);

            return services;
        }
    }
}
