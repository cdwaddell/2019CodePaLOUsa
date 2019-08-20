using System.Collections.Generic;

namespace MicroServiceDemo.Api.Blog.Data.Entities
{
    /// <summary>
    /// An entity that represents a unique article tag
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// The database assigned id of the tag
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The unique name of the tag
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Audit data for this tag
        /// </summary>
        public AuditRecord AuditRecord { get; set; }

        /// <summary>
        /// A collection of articles with this tag
        /// </summary>
        public virtual ICollection<ArticleTags> ArticleTags { get; set; }
            = new List<ArticleTags>();
    }
}