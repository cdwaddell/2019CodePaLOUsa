using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace MicroServiceDemo.Api.Blog.Logging
{
    /// <summary>
    /// Adds telemetry to application insights
    /// </summary>
    public class CorrelationTelemetry : ITelemetryInitializer
    {
        IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes correlation telemetry
        /// </summary>
        /// <param name="httpContextAccessor">The accessor to get data concerning the current request</param>
        public CorrelationTelemetry(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// When something needs to be sent to AI, this will add extra data to the request
        /// </summary>
        /// <param name="telemetry"></param>
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.GlobalProperties.Add("ApplicationName", "ApplicationInsightsTester");
        }
    }
}
