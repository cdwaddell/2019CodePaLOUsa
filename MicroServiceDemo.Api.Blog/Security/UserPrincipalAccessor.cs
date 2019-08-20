using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MicroServiceDemo.Api.Blog.Security
{
    /// <summary>
    /// Used to access a strongly typed user object
    /// </summary>
    public class UserPrincipalAccessor
    {
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Construct a user principal accessor
        /// </summary>
        /// <param name="contextAccessor">The http context accessor</param>
        public UserPrincipalAccessor(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Get the currently logged in user object
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<UserPrincipal> CurrentUser(CancellationToken cancellationToken = new CancellationToken())
        {
            var sub = _contextAccessor?.HttpContext?
                .User.FindFirst("sub")?.Value;

            if (sub == null)
                return null;

            var _cache = _contextAccessor.HttpContext
                .RequestServices.GetRequiredService<IUserCache>();

            var user = await _cache.GetUserBySub(sub, cancellationToken);
            return user;
        }
    }
}