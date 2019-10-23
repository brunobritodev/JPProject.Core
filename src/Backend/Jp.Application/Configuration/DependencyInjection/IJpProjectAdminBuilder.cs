using Microsoft.Extensions.DependencyInjection;

namespace Jp.Application.Configuration.DependencyInjection
{
    public interface IJpProjectAdminBuilder
    {
        /// <summary>Gets the services.</summary>
        /// <value>The services.</value>
        IServiceCollection Services { get; }
        JpProjectAdminOptions Options { get; }
    }
}