using System;
using MicroServiceDemo.Api.Blog.Models;

namespace MicroServiceDemo.Api.Blog.Bus
{
    public interface IUserUpdated: IDisposable
    {
        void ReceiveUpdate(UserDto user);
    }
}