using JPProject.Admin.Fakers.Test;
using JPProject.Admin.IntegrationTests.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace JPProject.Admin.IntegrationTests.ConfigurationTests
{
    /// <summary>
    /// It only work if databases are well configured
    /// </summary>

    public class DatabaseTests : IClassFixture<ConfigurationWarmup>
    {
        public ConfigurationWarmup Configuration { get; }

        public DatabaseTests(ConfigurationWarmup configuration)
        {
            Configuration = configuration;
        }

        [Trait("Category", "Database")]
        [Fact(
            Skip = "Database need to be configured to run. Usually works in tests scenarios"
        )]
        public void ShouldCreateServiceDatabase()
        {
            void DatabaseOptions(DbContextOptionsBuilder opt) => opt.UseInMemoryDatabase("JpTests").EnableSensitiveDataLogging();
            Configuration.ServiceCollection
                .AddDbContext<EventStoreContext>(DatabaseOptions);

            Configuration.ServiceCollection
                .ConfigureJpAdmin<AspNetUserTest>()
                .AddAdminContext(DatabaseOptions)
                .AddEventStore<EventStoreContext>();

            Configuration.ServiceCollection.BuildServiceProvider();

        }
    }
}
