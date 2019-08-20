namespace MicroServiceDemo.Api.Blog.Data.Entities
{
    /// <summary>
    /// A crosswalk entity for tagged articles
    /// </summary>
    public class ArticleTags{
        /// <summary>
        /// The database assigned id of this entity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Id of the article
        /// </summary>
        public int ArticleId { get; set; }

        /// <summary>
        /// The Id fo the tag
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// A navigation property for the tagged article
        /// </summary>
        public virtual Article Article { get; set; }

        /// <summary>
        /// A navigation property for the assigned tag
        /// </summary>
        public virtual Tag Tag { get; set; }
    }
}