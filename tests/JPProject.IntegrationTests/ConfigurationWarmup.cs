using System.IO;
using AutoMapper;
using JPProject.Admin.Application.AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace JPProject.Admin.IntegrationTests
{
    public class ConfigurationWarmup
    {
        public ConfigurationWarmup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app.json", optional: true, reloadOnChange: true);

            this.Configuration = builder.Build();

            var teste = Configuration.GetConnectionString("SqlServer");
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            this.ServiceCollection = new ServiceCollection();

            var mappings = AdminUiMapperConfiguration.RegisterMappings();
            var automapperConfig = new MapperConfiguration(mappings);
            ServiceCollection.TryAddSingleton(automapperConfig.CreateMapper());
            ServiceCollection.AddMediatR(typeof(WarmupInMemory));
            ServiceCollection.TryAddSingleton<IHttpContextAccessor>(mockHttpContextAccessor.Object);
        }

        public IConfigurationRoot Configuration { get; set; }

        public ServiceCollection ServiceCollection { get; set; }
    }
}
