namespace MicroServiceDemo.Api.Blog.Models
{
    /// <summary>
    /// A blog article
    /// </summary>
    public class ArticleDto: ArticlePostDto
    {
        /// <summary>
        /// The author of the article
        /// </summary>
        public UserDto Author { get; set; }

        /// <summary>
        /// A flag indicating that the current user has favors this article
        /// </summary>
        public bool Favorited { get; set; }

        /// <summary>
        /// A count of the number of favorites this article has
        /// </summary>
        public int FavoritesCount { get; set; }
    }
}