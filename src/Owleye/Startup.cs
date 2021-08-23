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
using EasyCaching.Core.Configurations;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GlobalExceptionHandler.WebApi;
using GlobalExceptionHandler;
using Newtonsoft.Json;
using System.Threading.Tasks;

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
            services.AddEasyCaching(options =>
            {
                options.UseRedis(config =>
                {
                    config.DBConfig.Endpoints.Add(new
                        ServerEndPoint(Configuration["General:RedisAddress"],
                        int.Parse(Configuration["General:RedisPort"])));
                }, Configuration["General:RedisInstanceName"]) //redis provider name
                .WithMessagePack()
                .UseRedisLock(); //with distributed lock, prevent problem in aysnc.
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Owleye API", Version = "v1" });
                c.EnableAnnotations();
            });

            services.AddTransient<ISensorService, SensorService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider sp, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddFile("Logs/Owleye-{Date}.log");

            if (env.IsDevelopment())
            {
                //TODO add development data.
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Owleye API V1"));
            }
            else
            {

                app.UseGlobalExceptionHandler(x =>
                {
                    x.ContentType = "application/json";
                    x.ResponseBody((s) => JsonConvert.SerializeObject(
                        new ExcepionResponseModel
                        {
                            Message = s.Message,
                        }));
                    x.OnException((c, logger) =>
                    {
                        logger.LogError("error in app," + c.Exception.Message);
                        return Task.CompletedTask;
                    });

                    x.Map<AppException>()
                    .ToStatusCode(x => x.ApiStatusCode)
                    .WithBody((ex, context) =>
                    JsonConvert.SerializeObject(new ExcepionResponseModel { Message = ex.Message, Code = ex.Code }));
                }, loggerFactory);

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
