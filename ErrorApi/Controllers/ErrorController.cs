using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ErrorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private ILogger _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        } 

        [HttpGet("500/{requestNum}")]
        public async Task<ActionResult<IEnumerable<string>>> Get([FromRoute] int requestNum, CancellationToken cancellationToken, [FromQuery] int timeout = 2)
        {
            _logger.LogWarning($"{requestNum}");
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(timeout), cancellationToken);
            }catch{}

            return StatusCode(500);
        }
    }
}
