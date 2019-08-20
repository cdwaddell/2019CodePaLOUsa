using System.Collections.Generic;

namespace MicroServiceDemo.Api.Blog.Data.Entities
{
    /// <summary>
    /// An entity that represents a user of the system, this user could be an author
    /// </summary>
    public class User
    {
        /// <summary>
        /// The database assigned id of the user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The username of the user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The bio of the user
        /// </summary>
        public string Bio { get; set; }

        /// <summary>
        /// An image for the user
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// A collection of articles that this user penned
        /// </summary>
        public virtual ICollection<Article> Articles { get; set; }
            = new List<Article>();

        /// <summary>
        /// A collection of this user's favorite articles
        /// </summary>
        public virtual ICollection<FavoriteArticle> FavoriteArticles { get; set; }
            = new List<FavoriteArticle>();

        /// <summary>
        /// Users that this user follows
        /// </summary>
        public virtual ICollection<FollowingUser> FollowingUsers { get; set; }
            = new List<FollowingUser>();

        /// <summary>
        /// Users that follow this user
        /// </summary>
        public virtual ICollection<FollowingUser> FollowedByUsers { get; set; }
            = new List<FollowingUser>();

        /// <summary>
        /// The external user id for this user
        /// </summary>
        public string Sub { get; set; }
    }
}