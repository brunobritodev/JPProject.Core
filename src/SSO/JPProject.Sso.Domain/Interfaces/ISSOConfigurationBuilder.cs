using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.Domain.Interfaces
{
    public interface ISsoConfigurationBuilder
    {
        IServiceCollection Services { get; }
        ISsoConfigurationBuilder AddCustomClaimsFactory<TFactory>() where TFactory : class;
    }
}
