using AutoMapper;
using AutoMapper.Configuration;
using JPProject.AspNet.Core;
using JPProject.Sso.Application.AutoMapper;
using JPProject.Sso.Application.Configuration;
using JPProject.Sso.Infra.Data.Configuration;
using JPProject.Sso.Infra.Identity.Models.Identity;
using JPProject.Sso.Infra.Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SSO.host.Context;
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

            void DatabaseOptions(DbContextOptionsBuilder opt) => opt.UseInMemoryDatabase("JpTests").EnableSensitiveDataLogging();

            services.AddDbContext<SsoContext>(DatabaseOptions);

            // ASP.NET Identity Configuration
            services
                .AddIdentity<UserIdentity, IdentityRole>(AccountOptions.NistAccountOptions)
                .AddEntityFrameworkStores<SsoContext>()
                .AddDefaultTokenProviders();

            // IdentityServer4 Configuration
            services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddAspNetIdentity<UserIdentity>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = DatabaseOptions;
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = DatabaseOptions;

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 15; // frequency in seconds to cleanup stale grants. 15 is useful during debugging
                });

            // SSO Configuration
            services
                .ConfigureSso<AspNetUser, UserService, RoleService>()
                .AddSsoContext<SsoContext>();


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
