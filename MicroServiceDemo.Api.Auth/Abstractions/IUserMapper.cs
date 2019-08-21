using MicroServiceDemo.Api.Auth.Data.Entities;
using MicroServiceDemo.Api.Auth.Models;
using MicroServiceDemo.Api.Blog.Abstractions;

namespace MicroServiceDemo.Api.Auth.Abstractions
{
    public interface IUserMapper: IMapperBase
    {
        /// <inheritdoc />
        T MapUser<T>(User user) where T: SimpleUserBaseDto, new();

        UserDto MapUser(UserRegisterDto source);

        User MapUser(UserDto user);

        void MapUser(UserDto userDto, User user);

        void MapUser(UserUpdateDto user, UserDto mapUser);
    }
}