using JPProject.Sso.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.Application.Configuration.DependencyInjection
{
    public class SsoBuilder : ISsoConfigurationBuilder
    {
        public SsoBuilder(IServiceCollection services, IdentityBuilder identityBuilder)
        {
            Services = services;
            IdentityBuilder = identityBuilder;
        }

        public IServiceCollection Services { get; }
        public IdentityBuilder IdentityBuilder { get; }

        public ISsoConfigurationBuilder AddCustomClaimsFactory<TFactory>() where TFactory : class
        {
            IdentityBuilder.AddClaimsPrincipalFactory<TFactory>();
            return this;
        }
    }
}