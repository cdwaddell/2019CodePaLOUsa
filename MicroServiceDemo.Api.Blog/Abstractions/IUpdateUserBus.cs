using MicroServiceDemo.Api.Blog.Models;

namespace MicroServiceDemo.Api.Auth.Bus
{
    public interface IUpdateUserBus
    {
        void SendUpdate(UserDto user);
    }
}