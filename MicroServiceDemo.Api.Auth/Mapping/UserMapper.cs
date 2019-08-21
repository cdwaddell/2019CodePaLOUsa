using System;
using MicroServiceDemo.Api.Auth.Abstractions;
using MicroServiceDemo.Api.Auth.Data.Entities;
using MicroServiceDemo.Api.Auth.Models;

namespace MicroServiceDemo.Api.Auth.Mapping
{
    /// <summary>
    /// Mapper for user objects
    /// </summary>
    public class UserMapper : IUserMapper
    {
        /// <inheritdoc />
        public T MapUser<T>(User user) where T: SimpleUserBaseDto, new()
        {
            var destination = new T();
            switch (destination)
            {
                case UserDto userDto:
                    MapUser(user, userDto);
                    break;
                case UserBaseDto userBaseDto:
                    MapUser(user, userBaseDto);
                    break;
                case SimpleUserBaseDto simpleUserBaseDto:
                    MapUser(user, simpleUserBaseDto);
                    break;
            }

            return destination;
        }

        public UserDto MapUser(UserRegisterDto source)
        {
            return new UserDto
            {
                Username = source.Username,
                Email = source.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public void MapUser(UserUpdateDto user, UserDto mapUser)
        {
            mapUser.UpdatedAt = DateTime.UtcNow;
            mapUser.Bio = user.Bio;
            mapUser.Image = user.Image;
            mapUser.Email = user.Email;
        }

        public void MapUser(UserDto user, User mapUser)
        {
            mapUser.Username = user.Username;
            mapUser.Email = user.Email;
            mapUser.Bio = user.Bio;
            mapUser.Image = user.Image;
            mapUser.UpdatedAt = user.UpdatedAt;
            mapUser.CreatedAt = user.CreatedAt;
        }

        public User MapUser(UserDto userDto)
        {
            var user = new User
            {
                Id = userDto.Id
            };
            MapUser(userDto, user);
            return user;
        }

        private void MapUser(User user, SimpleUserBaseDto simpleUserBaseDto)
        {
            simpleUserBaseDto.Username = user.Username;
            simpleUserBaseDto.Email = user.Email;
        }

        private void MapUser(User user, UserBaseDto userBaseDto)
        {
            userBaseDto.Bio = user.Bio;
            userBaseDto.Image = user.Image;

            MapUser(user, (SimpleUserBaseDto) userBaseDto);
        }

        private void MapUser(User user, UserDto userDto)
        {
            userDto.CreatedAt = user.CreatedAt;
            userDto.Id = user.Id;
            userDto.UpdatedAt = user.UpdatedAt;

            MapUser(user, (UserBaseDto) userDto);
        }
    }
}