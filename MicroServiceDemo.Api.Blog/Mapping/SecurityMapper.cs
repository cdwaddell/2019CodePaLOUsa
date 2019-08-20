using System.Linq;
using MicroServiceDemo.Api.Blog.Abstractions;
using MicroServiceDemo.Api.Blog.Data.Entities;
using MicroServiceDemo.Api.Blog.Security;

namespace MicroServiceDemo.Api.Blog.Mapping
{
    /// <summary>
    /// Mapper for security objects
    /// </summary>
    public class SecurityMapper: ISecurityMapper
    {
        /// <inheritdoc />
        public UserPrincipal MapUserPrincipal(User user)
        {
            return new UserPrincipal
            {
                Id = user.Id,
                FavoriteArticles = user.FavoriteArticles.Select(fa => fa.ArticleId).ToHashSet(),
                FollowedUsers = user.FollowingUsers.Select(fu => fu.UserId).ToHashSet(),
                Username = user.Username
            };
        }
    }
}