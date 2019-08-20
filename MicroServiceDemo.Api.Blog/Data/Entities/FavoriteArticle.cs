namespace MicroServiceDemo.Api.Blog.Data.Entities
{
    /// <summary>
    /// A crosswalk entity for favorite articles for a user
    /// </summary>
    public class FavoriteArticle
    {
        /// <summary>
        /// The database assigned id of this entity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The id of the user who has favorited this article
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The id of the favorted article
        /// </summary>
        public int ArticleId { get; set; }

        /// <summary>
        /// A navigation property for the article
        /// </summary>
        public virtual Article Article { get; set; }

        /// <summary>
        /// A navigation property for the user
        /// </summary>
        public virtual User User { get; set; }
    }
}