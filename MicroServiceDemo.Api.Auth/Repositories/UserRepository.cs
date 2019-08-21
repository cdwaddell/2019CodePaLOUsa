using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Auth.Abstractions;
using MicroServiceDemo.Api.Auth.Controllers.API.V1;
using MicroServiceDemo.Api.Auth.Data.Entities;
using MicroServiceDemo.Api.Auth.Models;
using MicroServiceDemo.Api.Auth.Security;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceDemo.Api.Auth.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly IBlogDbContext _context;
        private readonly IUserMapper _mapper;
        private readonly IPasswordHashService _hashService;

        public UserRepository(IBlogDbContext context, IUserMapper mapper, IPasswordHashService hashService)
        {
            _context = context;
            _mapper = mapper;
            _hashService = hashService;
        }

        public async Task<bool> DeleteByKeyAsync(string email, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await GetUser(email, cancellationToken);
            if (user == null)
                return false;
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync(cancellationToken) == 1;
        }

        public async Task<UserDto> GetByKeyAsync(string email, CancellationToken cancellationToken = new CancellationToken())
        {
            var user =await GetUser(email, cancellationToken);
            return _mapper.MapUser<UserDto>(user);
        }

        public async Task<UserDto> AddAsync(UserDto userDto, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = _mapper.MapUser(userDto);
            user.Id = 0;
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.MapUser<UserDto>(user);
        }

        public async Task<UserDto> UpdateAsync(string email, UserDto userDto, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await GetUser(email, cancellationToken);
            _mapper.MapUser(userDto, user);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.MapUser<UserDto>(user);
        }

        public async Task SetPassword(UserDto userDto, string password, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await GetUser(userDto.Email, cancellationToken);
            if (user.Salt == null)
                user.Salt = _hashService.GenerateSalt();
            user.PasswordHash = _hashService.ComputeHash(password, user.Salt);
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        public async Task<bool> CheckPassword(string email, string password, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await GetUser(email, cancellationToken);
            return _hashService.VerifyPassword(password, user.Salt, user.PasswordHash);
        }

        public async Task<UserDto> GetByUserNameAsync(string username, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username, cancellationToken);
            return _mapper.MapUser<UserDto>(user);
        }

        private async Task<User> GetUser(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
        }
    }
}
