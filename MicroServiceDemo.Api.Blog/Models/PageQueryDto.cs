using System.ComponentModel.DataAnnotations;

namespace MicroServiceDemo.Api.Blog.Models
{
    /// <summary>
    /// Page query parameters
    /// </summary>
    public class PageQueryDto
    {
        /// <summary>
        /// The number of items to skip
        /// </summary>
        [Range(0,10000)]
        public int Offset { get; set; }

        /// <summary>
        /// The number of items to get
        /// </summary>
        [Range(0,100)]
        public int Limit { get; set; }

        /// <summary>
        /// The tag to search for
        /// </summary>
        public string Tag { get; set; }
    }
}
