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
        /// A collection of comments that this user penned
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }
            = new List<Comment>();
    }
}