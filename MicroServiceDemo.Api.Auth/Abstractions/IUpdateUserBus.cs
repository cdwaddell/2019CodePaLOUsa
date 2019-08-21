using MicroServiceDemo.Api.Auth.Models;

namespace MicroServiceDemo.Api.Auth.Bus
{
    public interface IUpdateUserBus
    {
        void SendUpdate(UserDto user);
    }
}