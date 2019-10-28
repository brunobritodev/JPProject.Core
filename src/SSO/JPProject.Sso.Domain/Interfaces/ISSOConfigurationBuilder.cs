using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.Domain.Interfaces
{
    public interface ISSOConfigurationBuilder
    {
        IServiceCollection Services { get; }
    }
}
