using Microsoft.Extensions.DependencyInjection;

namespace JPProject.Sso.Integration.Tests
{
    public interface IWarmupTest
    {
        ServiceProvider Services { get; set; }
    }
}