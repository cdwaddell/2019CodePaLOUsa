using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceDemo.Api.Auth.Controllers.API.V1
{
    public class UserContainerDto
    {
        public SimpleUserBaseDto User { get; set; }
    }

    public class SimpleUserBaseDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
    }

    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserRegisterDto : SimpleUserBaseDto
    {
        public string Password { get; set; }
    }

    public class UserBaseDto : SimpleUserBaseDto
    {
        public object Bio { get; set; }
        public object Image { get; set; }
    }

    public class UserDto : UserBaseDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Token { get; set; }
    }

    public class UserUpdateDto : UserBaseDto
    {
        public string Password { get; set; }
    }

    public class UsersController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<UserDto>> Post([FromBody] UserRegisterDto user)
        {
            throw new NotImplementedException();
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Post([FromBody] UserLoginDto user)
        {
            throw new NotImplementedException();
        }
    }

    public class UserController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<UserDto>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<ActionResult<UserDto>> Put()
        {
            throw new NotImplementedException();
        }
    }
}
