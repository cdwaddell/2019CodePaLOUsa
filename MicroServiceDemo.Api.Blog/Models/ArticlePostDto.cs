using System;
using MicroServiceDemo.Api.Blog.Abstractions;

namespace MicroServiceDemo.Api.Blog.Models
{
    /// <summary>
    /// A blog article
    /// </summary>
    public class ArticlePostDto: IAuditableDto
    {
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

        /// <inheritdoc />
        public DateTime CreatedAt { get; set; }


        /// <inheritdoc />
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// The list of tags associated with this article
        /// </summary>
        public string[] TagList { get; set; }
    }
}