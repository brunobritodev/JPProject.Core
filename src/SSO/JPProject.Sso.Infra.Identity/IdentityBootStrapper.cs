using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Identity.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityBootStrapper
    {
        public static ISsoConfigurationBuilder AddDefaultAspNetIdentityServices(this ISsoConfigurationBuilder services)
        {
            // Infra - Identity Services
            services.Services.AddTransient<IUserService, UserService>();
            services.Services.AddTransient<IRoleService, RoleService>();

            return services;
        }
    }
}
