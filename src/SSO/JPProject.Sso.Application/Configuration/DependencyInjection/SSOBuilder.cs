using JPProject.Sso.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.Application.Configuration.DependencyInjection
{
    public class SsoBuilder : ISsoConfigurationBuilder
    {
        public SsoBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

    }
}