using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Auth.Models;
using MicroServicesDemo.Abstractions;

namespace MicroServiceDemo.Api.Auth.Abstractions
{
    public interface IUserRepository : IRepository<UserDto, string>
    {
        Task<bool> CheckPassword(string email, string password, CancellationToken cancellationToken = new CancellationToken());
        Task SetPassword(UserDto userDto, string password, CancellationToken cancellationToken = new CancellationToken());
        Task<UserDto> GetByUserNameAsync(string username, CancellationToken cancellationToken = new CancellationToken());
    }
}