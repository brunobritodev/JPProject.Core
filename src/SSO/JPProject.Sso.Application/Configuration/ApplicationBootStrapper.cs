using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.Application.Configuration
{
    internal static class ApplicationBootStrapper
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<IUserManageAppService, UserManagerAppService>();
            services.AddScoped<IRoleManagerAppService, RoleManagerAppService>();

            return services;
        }
    }
}
