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
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

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

            services.AddControllers();

            services.AddApiVersioning(
               options =>
               {
                   options.ReportApiVersions = true;
                   options.DefaultApiVersion = new ApiVersion(1, 0);
                   options.AssumeDefaultVersionWhenUnspecified = true;
               });

            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });


            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(x => x.FullName);
                options.DescribeAllParametersInCamelCase();
                options.ResolveConflictingActions(o => o.FirstOrDefault());

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "api.swagger.Versioning", Version = "v1" });
                options.SwaggerDoc("v2", new OpenApiInfo { Title = "api.swagger.Versioning", Version = "v2" });
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

            services.AddTransient<ISensorService, SensorService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider sp, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider provider)
        {
            //loggerFactory.AddFile("Logs/Owleye-{Date}.log");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();


            }

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.DisplayRequestDuration();

                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                        "owleye " + desc.GroupName.ToUpper());
                }
            });


            app.UseRouting();

            app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials());


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //TODO : ANTI Pattern, Refactor THIS
            var serviceScope = app.ApplicationServices.
                GetRequiredService<IServiceScopeFactory>().CreateScope();
            ServiceLocator.Init(serviceScope.ServiceProvider);

            QuartzBootStrap.Boot();
        }

    }

}
