using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Abstractions;
using MicroServiceDemo.Api.Blog.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace MicroServiceDemo.Api.Blog.Cache
{
    /// <summary>
    /// A cache for storing user information
    /// </summary>
    public class UserCache : IUserCache
    {
        private readonly IBlogDbContext _context;
        private readonly ISecurityMapper _mapper;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Initialize a new user cache instance
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="mapper"></param>
        /// <param name="context"></param>
        public UserCache(IMemoryCache cache, ISecurityMapper mapper, IBlogDbContext context)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        /// <summary>
        /// Get a user by an external unique identifier provided by a token
        /// </summary>
        /// <param name="sub">The unique identifier</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The user principal</returns>
        public async Task<UserPrincipal> GetUserBySub(string sub, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _cache.GetOrCreateAsync($"User_{sub}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                var user = await _context.Users
                    .AsNoTracking()
                    .Include(x => x.FavoriteArticles)
                    .Include(x => x.FollowingUsers)
                    .Where(x => x.Sub == sub)
                    .SingleOrDefaultAsync(cancellationToken);

                return _mapper.MapUserPrincipal(user);
            });
        }

        /// <summary>
        /// Remove a user from cache so it can be rehydrated
        /// </summary>
        /// <param name="sub"></param>
        public void RemoveFromCache(string sub)
        {
            _cache.Remove($"User_{sub}");
        }
    }
}
