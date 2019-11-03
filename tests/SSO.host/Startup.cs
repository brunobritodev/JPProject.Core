using AutoMapper;
using AutoMapper.Configuration;
using JPProject.AspNet.Core;
using JPProject.Domain.Core.ViewModels;
using JPProject.Sso.Application.AutoMapper;
using JPProject.Sso.Database;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace SSO.host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var database = Configuration.GetValue<DatabaseType>("ApplicationSettings:DatabaseType");
            var connString = Configuration.GetConnectionString("SSOConnection");

            services
                .ConfigureUserIdentity<AspNetUser>().AddDatabase(database, connString)
                .ConfigureIdentityServer().AddOAuth2Database(database, connString);

            var configurationExpression = new MapperConfigurationExpression();
            SsoMapperConfig.RegisterMappings().ForEach(p => configurationExpression.AddProfile(p));
            var automapperConfig = new MapperConfiguration(configurationExpression);

            services.TryAddSingleton(automapperConfig.CreateMapper());
            // Adding MediatR for Domain Events and Notifications
            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
