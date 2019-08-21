using System;
using MassTransit;
using MicroServiceDemo.Api.Auth.Abstractions;
using MicroServiceDemo.Api.Auth.Controllers.API.V1;
using MicroServiceDemo.Api.Auth.Data;
using MicroServiceDemo.Api.Auth.Models;
using MicroServiceDemo.Api.Auth.Security;
using MicroServiceDemo.Api.Blog.Extensions;
using MicroServiceDemo.Api.Blog.Logging;
using MicroServiceDemo.Api.Blog.Security;
using MicroServiceDemo.Api.Blog.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection;

namespace MicroServiceDemo.Api.Auth
{
    /// <summary>
    /// Asp.Net Core startup 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration created by hosting builder</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Application configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            
            var assembly = GetType().Assembly;

            services.AddRepositories(assembly)
                .AddCache(assembly)
                .AddMapper(assembly);

            services.AddApplicationInsightsTelemetry();

            services.AddEntityFramework<IBlogDbContext, BlogDbContext>(Configuration);
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            services.AddSingleton<IPasswordHashService, PasswordHashService>();

            services.Configure<SwaggerSettings>(Configuration.GetSection(nameof(SwaggerSettings)));
            services.Configure<ApplicationMetadata>(Configuration.GetSection(nameof(ApplicationMetadata)));

            var appSettingsSection = Configuration.GetSection(nameof(AppSettings));
            services.Configure<AppSettings>(appSettingsSection);
            services.Configure<RabbitMqSettings>(Configuration.GetSection(nameof(RabbitMqSettings)));

            services.AddSingleton(c =>
            {
                var settings = c.GetService<RabbitMqSettings>();

                var factory = new ConnectionFactory
                {
                    HostName = settings.HostName,
                    Port = settings.Port,
                    UserName = settings.UserName,
                    Password = settings.Password,

                    AutomaticRecoveryEnabled = true,
                    RequestedConnectionTimeout = 5000,
                    TopologyRecoveryEnabled = true,
                    SocketReadTimeout = 10000,
                    SocketWriteTimeout = 10000,
                    ContinuationTimeout = TimeSpan.FromSeconds(10)
                };

                return factory;
            });

            services.AddSingleton(c =>
            {
                var factory = c.GetService<ConnectionFactory>();
                return factory.CreateConnection();
            });

            //using RabbitMQ.Client.Core.DependencyInjection
            //var rabbitMqSection = Configuration.GetSection("RabbitMq");
            //services.AddRabbitMqClient(rabbitMqSection);

            //using MassTransit
            //services.AddMassTransit(x =>
            //{
            //    //x.AddBus(provider => Bus.Factory.CreateUsingAzureServiceBus(x =>
            //    //{
            //    //    var host = x.Host(serviceUri, h =>
            //    //    {
            //    //        h.SharedAccessSignature(s =>
            //    //        {
            //    //            s.KeyName = "keyName";
            //    //            s.SharedAccessKey = "key";
            //    //            s.TokenTimeToLive = TimeSpan.FromDays(1);
            //    //            s.TokenScope = TokenScope.Namespace;
            //    //        });
            //    //    });
            //    //});

            //    //x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            //    //{
            //    //    cfg.Host(new Uri($"rabbitmq://{Configuration["RabbitMQHostName"]}"), hostConfig =>
            //    //    {
            //    //        hostConfig.Username("guest");
            //    //        hostConfig.Password("guest");
            //    //    });

            //    //    cfg.SendTopology
            //    //    cfg.UseMessageRetry(configurator => configurator.SetRetryPolicy());
            //    //}));
            //});

            services
                .AddJwtAuthentication(appSettingsSection)
                .AddMvc()
                .AddJsonOptions(opt => {
                    opt.SerializerSettings.ContractResolver = 
                        new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        };
                    opt.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddApiVersionWithExplorer()
                .AddSwaggerOptions()
                .AddSwaggerGen();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseSwaggerDocuments();
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
