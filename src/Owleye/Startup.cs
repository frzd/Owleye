using System;
using LiteX.Email;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Owleye.Shared.Cache;
using Owleye.Shared.Data;
using Owleye.Core;
using Owleye.Infrastructure.Cache;
using Owleye.Core.Handlers;
using Owleye.Infrastructure.Quartz;
using Owleye.Core.Services;
using Owleye.Infrastructure.Data;
using Owleye.Infrastructure.Service;

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
                options.UseSqlServer(Configuration.GetConnectionString(nameof(OwleyeDbContext))), ServiceLifetime.Transient);

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IRedisCache, RedisCache>();
            services.AddTransient<IQrtzSchedule, QrtzSchedule>();
            services.AddMediatR(typeof(DoPingHandler).Assembly);
            services.AddLiteXSmtpEmail();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["General:RedisAddress"];
            });

            services.AddTransient<ISensorService, SensorService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider sp, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddFile("Logs/Owleye-{Date}.log");

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

}
