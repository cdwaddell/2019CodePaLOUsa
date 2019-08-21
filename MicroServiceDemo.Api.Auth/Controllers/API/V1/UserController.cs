using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MicroServiceDemo.Api.Auth.Abstractions;
using MicroServiceDemo.Api.Auth.Models;
using MicroServiceDemo.Api.Auth.Repositories;
using MicroServiceDemo.Api.Blog.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceDemo.Api.Auth.Controllers.API.V1
{
    public class UserController : ApiControllerBase
    {
        private readonly IBus _bus;
        private readonly IUserMapper _mapper;
        private readonly IUserRepository _repository;
        private readonly IJwtTokenService _tokenService;

        public UserController(IBus bus, IUserMapper mapper, IUserRepository repository, IJwtTokenService tokenService)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> Get(CancellationToken cancellationToken)
        {
            var username = User.FindFirst("sub")?.Value;
            if (username == null)
                return Forbid();

            var user = await _repository.GetByUserNameAsync(username, cancellationToken);
            _tokenService.PopulateToken(user);
            return user;
        }

        [HttpPut]
        public async Task<ActionResult<UserDto>> Put(UserUpdateDto user, CancellationToken cancellationToken)
        {
            var username = User.FindFirst("sub")?.Value;
            if (username == null)
                return Forbid();

            var updated = await _repository.GetByUserNameAsync(user.Username, cancellationToken);
            _mapper.MapUser(user, updated);
            updated = await _repository.UpdateAsync(user.Email, updated, cancellationToken);
            await _repository.SetPassword(updated, user.Password, cancellationToken);
            _tokenService.PopulateToken(updated);

            return updated;
        }
    }
}
