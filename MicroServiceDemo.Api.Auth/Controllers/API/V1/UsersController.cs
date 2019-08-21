using MicroServiceDemo.Api.Auth.Abstractions;
using MicroServiceDemo.Api.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using MicroServicesDemo.WebApi;

namespace MicroServiceDemo.Api.Auth.Controllers.API.V1
{
    [AllowAnonymous]
    public class UsersController : ApiControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IUserMapper _userMapper;
        private readonly IJwtTokenService _tokenService;

        public UsersController(IUserRepository repository, IUserMapper userMapper, IJwtTokenService tokenService)
        {
            _repository = repository;
            _userMapper = userMapper;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Post([FromBody] UserRegisterDto user, CancellationToken cancellationToken)
        {
            var userDto = await _repository.AddAsync(_userMapper.MapUser(user), cancellationToken);
            await _repository.SetPassword(userDto, user.Password, cancellationToken);
            _tokenService.PopulateToken(userDto);
            return userDto;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Post([FromBody] UserLoginDto userDto, CancellationToken cancellationToken)
        {
            if (await _repository.CheckPassword(userDto.Email, userDto.Password, cancellationToken))
            {
                var user = await _repository.GetByKeyAsync(userDto.Email, cancellationToken);
                _tokenService.PopulateToken(user);
                return user;
            }
            else return BadRequest();
        }
    }
}