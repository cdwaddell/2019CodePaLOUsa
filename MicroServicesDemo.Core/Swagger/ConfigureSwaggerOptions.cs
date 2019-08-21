using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace MicroServicesDemo.Swagger
{
    /// <inheritdoc />
    public sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerOptions>
    {
        private readonly SwaggerSettings _settings;

        /// <inheritdoc />
        public ConfigureSwaggerOptions(IOptions<SwaggerSettings> settings)
        {
            _settings = settings?.Value ?? new SwaggerSettings();
        }

        /// <inheritdoc />
        public void Configure(SwaggerOptions options)
        {
            options.RouteTemplate = _settings.RoutePrefixWithSlash + "{documentName}/swagger.json";
        }
    }
}