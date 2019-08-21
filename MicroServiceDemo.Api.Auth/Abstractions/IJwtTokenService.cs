using MicroServiceDemo.Api.Auth.Models;

namespace MicroServiceDemo.Api.Auth.Abstractions
{
    public interface IJwtTokenService
    {
        void PopulateToken(UserDto user);
    }
}