using System;
using System.Linq;
using System.Reflection;
using System.Text;
using MicroServiceDemo.Api.Blog.Abstractions;
using MicroServiceDemo.Api.Blog.Security;
using MicroServiceDemo.Api.Blog.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace MicroServiceDemo.Api.Blog.Extensions
{
    /// <summary>
    /// Service Collection(IServiceCollection) Extensions
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// Add AddVersionedApiExplorer and AddApiVersioning middleware
        /// </summary>
        /// <param name="services"></param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddApiVersionWithExplorer(this IServiceCollection services)
        {
            return services
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                })
                .AddApiVersioning(options =>
                {
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });
        }

        /// <summary>
        /// Add entity framework services
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>/></param>
        /// <param name="configuration"></param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddEntityFramework<IT, T>(this IServiceCollection services, IConfiguration configuration) 
            where T:DbContext, IT where IT : class
        {
            var migrationAssembly = typeof(T).Assembly.GetName().Name;

            return services.AddEntityFrameworkSqlServer()
                .AddDbContext<T>((serviceProvider, options) =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("BlogDbContext"),
                        x => x.MigrationsAssembly(migrationAssembly)
                    ).UseInternalServiceProvider(serviceProvider))
                .AddScoped<IT, T>();
        }

        /// <summary>
        /// Add swagger services
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>/></param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddSwaggerOptions(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureOptions<SwaggerOptions>, ConfigureSwaggerOptions>()
                .AddTransient<IConfigureOptions<SwaggerUIOptions>, ConfigureSwaggerUiOptions>()
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
        }

        /// <summary>
        /// Add generic repositories to the IOC 
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configSection">The configuration section</param>
        /// <returns></returns>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfigurationSection configSection)
        {
            // configure jwt authentication
            var appSettings = configSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }

        /// <summary>
        /// Add generic repositories to the IOC 
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="assembly">The assembly to scan</param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services, Assembly assembly)
        {
            var interfaceType = typeof(IRepositoryBase);

            return ServiceCollection(services, assembly, interfaceType);
        }

        /// <summary>
        /// Add generic cache to the IOC 
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="assembly">The assembly to scan</param>
        /// <returns></returns>
        public static IServiceCollection AddCache(this IServiceCollection services, Assembly assembly)
        {
            var interfaceType = typeof(ICacheBase);

            return ServiceCollection(services, assembly, interfaceType);
        }

        /// <summary>
        /// Add generic mappers to the IOC 
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="assembly">The assembly to scan</param>
        /// <returns></returns>
        public static IServiceCollection AddMapper(this IServiceCollection services, Assembly assembly)
        {
            var interfaceType = typeof(IMapperBase);

            return ServiceCollection(services, assembly, interfaceType);
        }

        private static IServiceCollection ServiceCollection(IServiceCollection services, Assembly assembly, Type interfaceType)
        {
            var typesToRegister = assembly
                .GetTypes()
                .Where(x =>
                    !string.IsNullOrEmpty(x.Namespace)
                    && x.IsClass
                    && interfaceType.IsAssignableFrom(x))
                .ToList();

            foreach (var type in typesToRegister)
            {
                var repositoryInterfaces = type.GetInterfaces()
                    .Where(interfaceType.IsAssignableFrom);

                foreach (var repositoryInterface in repositoryInterfaces)
                {
                    services.AddTransient(repositoryInterface, type);
                }
            }

            return services;
        }
    }
}
