using System.Collections.Generic;

namespace MicroServiceDemo.Api.Blog.Security
{
    /// <summary>
    /// A logged in user object
    /// </summary>
    public class UserPrincipal
    {
        /// <summary>
        /// The user's database Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The external id of the user
        /// </summary>
        public string Sub { get; set; }

        /// <summary>
        /// The username of the user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// A flag indicating that this user is an author
        /// </summary>
        public bool IsAuthor { get; set; }

        /// <summary>
        /// A set of favorite articles
        /// </summary>
        public HashSet<int> FavoriteArticles { get; set; }

        /// <summary>
        /// A set of followed users
        /// </summary>
        public HashSet<int> FollowedUsers { get; set; }
    }
}
