using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Identity.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityBootStrapper
    {
        public static ISsoConfigurationBuilder AddDefaultAspNetIdentityServices(this ISsoConfigurationBuilder services)
        {
            // Infra - Identity Services
            services.Services.AddScoped<IUserService, UserService>();
            services.Services.AddScoped<IRoleService, RoleService>();

            return services;
        }
    }
}
