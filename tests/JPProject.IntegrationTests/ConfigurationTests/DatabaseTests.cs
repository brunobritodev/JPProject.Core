using JPProject.Admin.Fakers.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        //Skip = "Database need to be configured to run. Usually works in tests scenarios"
        )]
        public void ShouldConnectSqlServerDatabase()
        {
            var connString = Configuration.Configuration.GetConnectionString("SqlServer");
            Configuration.ServiceCollection.ConfigureJpAdmin<AspNetUserTest>().WithSqlServer(opt => opt.UseSqlServer(connString).EnableSensitiveDataLogging());

            Configuration.ServiceCollection.BuildServiceProvider();
        }
    }
}
