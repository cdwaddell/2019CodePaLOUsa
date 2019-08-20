using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceDemo.Api.Blog.Controllers.API.V1
{
    public class ProfilesController: ApiControllerBase
    {
        [HttpPost("{username}/follow")]
        public async Task<ActionResult> Follow([FromRoute]string username) {
            throw new NotImplementedException();
        }

        [HttpDelete("{username}/follow")]
        public async Task<ActionResult> Unfollow([FromRoute]string username) {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromRoute]string username) {
            throw new NotImplementedException();
        }
    }
}
