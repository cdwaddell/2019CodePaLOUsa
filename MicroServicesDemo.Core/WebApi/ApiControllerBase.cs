using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceDemo.Api.Blog.WebApi
{    
    /// <summary>
    /// Base API controller class for auto-assigning routes
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    public abstract class 
        ApiControllerBase:ControllerBase
    {
    }
}
