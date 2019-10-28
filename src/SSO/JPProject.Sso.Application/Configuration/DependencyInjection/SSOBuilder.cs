using JPProject.Sso.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.Application.Configuration.DependencyInjection
{
    public class SSOBuilder : ISSOConfigurationBuilder
    {
        public SSOBuilder(IServiceCollection services)
        {
            Services = services;
        }
        public IServiceCollection Services { get; }
    }
}