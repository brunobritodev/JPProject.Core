using JPProject.Sso.Application.CloudServices.Storage;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.Application.Configuration
{
    internal static class IdentityBootStrapper
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            // Infra - Identity Services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddScoped<IStorage, StorageService>();

            return services;
        }
    }
}
