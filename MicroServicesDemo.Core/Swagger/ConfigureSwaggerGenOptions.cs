using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MicroServicesDemo.Swagger
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation of IConfigureOptions&lt;SwaggerGenOptions&gt;
    /// </summary>
    public sealed class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly SwaggerSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerGenOptions"/> class.
        /// </summary>
        /// <param name="versionDescriptionProvider">IApiVersionDescriptionProvider</param>
        /// <param name="swaggerSettings">App Settings for Swagger</param>
        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider versionDescriptionProvider,
            IOptions<SwaggerSettings> swaggerSettings)
        {
            Debug.Assert(versionDescriptionProvider != null, $"{nameof(versionDescriptionProvider)} != null");
            Debug.Assert(swaggerSettings != null, $"{nameof(swaggerSettings)} != null");

            _provider = versionDescriptionProvider;
            _settings = swaggerSettings.Value ?? new SwaggerSettings();
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            options.OperationFilter<SwaggerDefaultValues>();
            
            options.DescribeAllEnumsAsStrings();
            options.IgnoreObsoleteActions();
            options.IgnoreObsoleteProperties();

            AddSwaggerDocumentForEachDiscoveredApiVersion(options);
            SetCommentsPathForSwaggerJsonAndUi(options);
        }

        private void AddSwaggerDocumentForEachDiscoveredApiVersion(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                _settings.Info.Version = description.ApiVersion.ToString();

                if (description.IsDeprecated)
                {
                    _settings.Info.Description += " - DEPRECATED";
                }

                options.SwaggerDoc(description.GroupName, _settings.Info);
            }
        }

        private static void SetCommentsPathForSwaggerJsonAndUi(SwaggerGenOptions options)
        {
            var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        }
    }
}