using JPProject.Sso.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JPProject.Sso.Infra.Data.Configuration
{
    public static class ContextConfiguration
    {
        public static IServiceCollection AddSsoContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.AddDbContext<ApplicationIdentityContext>(optionsAction);

            return services;
        }
    }
}
