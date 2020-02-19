namespace Microsoft.Extensions.DependencyInjection
{
    public interface IJpProjectConfigurationBuilder
    {
        IServiceCollection Services { get; }
    }
}
