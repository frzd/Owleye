using System;
using LiteX.Email;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Owleye.Common.Cache;
using Owleye.Model.Model;
using Owleye.Model.Repository;
using Owleye.Service;
using Owleye.Service.Bl;
using Owleye.Service.Notifications.Services;

namespace Owleye
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<OwleyeDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(nameof(OwleyeDbContext))));

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IRedisCache, RedisCache>();
            services.AddMediatR(typeof(DoPingServiceHandler).Assembly);
            services.AddLiteXSmtpEmail();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });

            services.AddTransient<ISensorService, SensorService>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider sp)
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

            //TODO : ANTI Pattern, Refactor THIS
            var serviceScope = app.ApplicationServices.
                GetRequiredService<IServiceScopeFactory>().CreateScope();
            ServiceLocator.Init(serviceScope.ServiceProvider);

            new QuartzBootStrap().Boot();
        }
    }

    /// <summary>
    ///  temporary usage of this pattern 
    /// </summary>
    public static class ServiceLocator
    {
        private static IServiceProvider _provider;
        public static void Init(IServiceProvider provider)
        {
            _provider = provider;
        }

        public static T Resolve<T>() => (T)_provider.GetService(typeof(T));
    }
}
