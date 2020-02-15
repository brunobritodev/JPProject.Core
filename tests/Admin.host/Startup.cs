using AutoMapper;
using AutoMapper.Configuration;
using JPProject.Admin.Application.AutoMapper;
using JPProject.AspNet.Core;
using JPProject.Sso.Integration.Tests.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Admin.host
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

            void Options(DbContextOptionsBuilder opt) => opt.UseInMemoryDatabase("JpTests").EnableSensitiveDataLogging();

            services.AddDbContext<EventStoreContext>(Options);
            services
                .ConfigureJpAdmin<AspNetUser>()
                .AddEventStore<EventStoreContext>()
                .AddAdminContext(Options);


            var configurationExpression = new MapperConfigurationExpression();
            AdminUiMapperConfiguration.RegisterMappings().ForEach(p => configurationExpression.AddProfile(p));
            var automapperConfig = new MapperConfiguration(configurationExpression);

            services.TryAddSingleton(automapperConfig.CreateMapper());
            // Adding MediatR for Domain Events and Notifications
            services.AddMediatR(typeof(Startup));

            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private void RegisterServices(IServiceCollection services)
        {
            // Adding dependencies from another layers (isolated from Presentation)
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }
    }
}
