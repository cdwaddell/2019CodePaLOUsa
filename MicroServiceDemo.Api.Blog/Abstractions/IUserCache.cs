using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Security;

namespace MicroServiceDemo.Api.Blog.Abstractions
{
    /// <summary>
    /// A cache for storing user information
    /// </summary>
    public interface IUserCache: ICacheBase
    {
        /// <summary>
        /// Get a user by an external unique identifier provided by a token
        /// </summary>
        /// <param name="sub">The unique identifier</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The user principal</returns>
        Task<UserPrincipal> GetUserBySub(string sub, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Remove a user from cache so it can be rehydrated
        /// </summary>
        /// <param name="sub"></param>
        void RemoveFromCache(string sub);
    }
}