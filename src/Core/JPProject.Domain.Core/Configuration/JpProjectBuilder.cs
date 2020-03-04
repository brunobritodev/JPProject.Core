using JPProject.Domain.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Domain.Core.Configuration
{
    public class JpProjectBuilder : IJpProjectConfigurationBuilder
    {
        public JpProjectBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
