namespace MicroServiceDemo.Api.Blog.Models
{
    /// <summary>
    /// A blog user
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// The author's username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The author's bio
        /// </summary>
        public string Bio { get; set; }

        /// <summary>
        /// An image url of the author
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// A flag indicating that the current user follows this author
        /// </summary>
        public bool Following { get; set; }
    }
}