using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Infra.Identity.Models.Identity;
using JPProject.Sso.Infra.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JPProject.Sso.Infra.Identity.Configuration
{
    public static class IdentityConfiguration
    {
        public static ISsoConfigurationBuilder ConfigureIdentity<TUser, TRole, TKey, TRoleFactory, TUserFactory>(this ISsoConfigurationBuilder services)
            where TUser : IdentityUser<TKey>, IDomainUser
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
            where TRoleFactory : class, IRoleFactory<TRole>
            where TUserFactory : class, IIdentityFactory<TUser>
        {
            services.Services.AddScoped<IUserService, UserService<TUser, TKey>>();
            services.Services.AddScoped<IRoleService, RoleService<TRole, TKey>>();
            services.Services.AddScoped<IIdentityFactory<TUser>, TUserFactory>();
            services.Services.AddScoped<IRoleFactory<TRole>, TRoleFactory>();

            return services;
        }

        public static ISsoConfigurationBuilder AddDefaultAspNetIdentityServices(this ISsoConfigurationBuilder services)
        {
            // Infra - Identity Services
            services.Services.AddScoped<IUserService, UserService<UserIdentity, string>>();
            services.Services.AddScoped<IRoleService, RoleService<RoleIdentity, string>>();
            services.Services.AddScoped<IIdentityFactory<UserIdentity>, IdentityFactory>();
            services.Services.AddScoped<IRoleFactory<RoleIdentity>, IdentityFactory>();

            return services;
        }

    }
}
