using System.Collections.Generic;

namespace MicroServiceDemo.Api.Blog.Models
{
    /// <summary>
    /// A paged article collection in this blog
    /// </summary>
    public class ArticlesDto
    {
        /// <summary>
        /// The blog articles for this page
        /// </summary>
        public List<ArticleDto> Articles { get; set; }

        /// <summary>
        /// The total number of articles in the entire collection
        /// </summary>
        public int ArticlesCount { get; set; }
    }
}