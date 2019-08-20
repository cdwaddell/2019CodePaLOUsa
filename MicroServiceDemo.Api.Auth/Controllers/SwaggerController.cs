using Microsoft.AspNetCore.Mvc;

namespace MicroServiceDemo.Api.Auth.Controllers
{
    /// <summary>
    /// Controller to manage directing a user to swagger
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SwaggerController: ControllerBase
    {
        /// <summary>
        /// Send the user to the swagger page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public RedirectResult DefaultRedirect()
        {
            return Redirect("~/index.html");
        }
    }
}
