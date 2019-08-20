using System.Collections.Generic;

namespace MicroServiceDemo.Api.Blog.Data.Entities
{
    /// <summary>
    /// An article entity
    /// </summary>
    public class Article
    {
        /// <summary>
        /// The database assigned id of the article
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The article title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The unique identifier of the article
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// The HTML body of the article
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The description of the article
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A count of the number of favorites this article has
        /// </summary>
        public int FavoritesCount { get; set; }

        /// <summary>
        /// Audit data
        /// </summary>
        public AuditRecord AuditRecord { get; set; }
            = new AuditRecord();

        /// <summary>
        /// The author Id of the article
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// The author of the article
        /// </summary>
        public virtual User Author { get; set; }

        /// <summary>
        /// A navigation property for assigned tags
        /// </summary>
        public virtual ICollection<ArticleTags> ArticleTags { get; set; } 
            = new List<ArticleTags>();

        /// <summary>
        /// A navigation property to find users who have favorited this article
        /// </summary>
        public virtual ICollection<FavoriteArticle> FavoredBy { get; set; }
            = new List<FavoriteArticle>();
    }
}