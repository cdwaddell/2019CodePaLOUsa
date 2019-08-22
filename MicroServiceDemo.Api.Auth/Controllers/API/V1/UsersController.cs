using MicroServiceDemo.Api.Auth.Abstractions;
using MicroServiceDemo.Api.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Auth.Bus;
using MicroServicesDemo.WebApi;

namespace MicroServiceDemo.Api.Auth.Controllers.API.V1
{
    [AllowAnonymous]
    public class UsersController : ApiControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IUserMapper _userMapper;
        private readonly IJwtTokenService _tokenService;
        private readonly IUpdateUserBus _bus;

        public UsersController(
            IUserRepository repository, 
            IUserMapper userMapper, 
            IJwtTokenService tokenService,
            IUpdateUserBus bus)
        {
            _repository = repository;
            _userMapper = userMapper;
            _tokenService = tokenService;
            _bus = bus;
        }

        [HttpPost]
        public async Task<ActionResult<UserContainerDto<UserDto>>> Post([FromBody] UserContainerDto<UserRegisterDto> container, CancellationToken cancellationToken)
        {
            var user = container.User;
            var userDto = await _repository.AddAsync(_userMapper.MapUser(user), cancellationToken);
            await _repository.SetPassword(userDto, user.Password, cancellationToken);
            _tokenService.PopulateToken(userDto);
            _bus.SendUpdate(userDto);
            return new UserContainerDto<UserDto>
            {
                User = userDto
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserContainerDto<UserDto>>> Post([FromBody] UserLoginContainerDto container, CancellationToken cancellationToken)
        {
            var userDto = container.User;
            if (await _repository.CheckPassword(userDto.Email, userDto.Password, cancellationToken))
            {
                var user = await _repository.GetByKeyAsync(userDto.Email, cancellationToken);
                _tokenService.PopulateToken(user);
                return new UserContainerDto<UserDto>
                {
                    User = user
                };
            }
            return BadRequest();
        }
    }
}