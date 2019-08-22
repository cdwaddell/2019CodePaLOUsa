using System;

namespace MicroServiceDemo.Api.Blog.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Body { get; set; }
        public virtual User Author { get; set; }
        public int AuthorId { get; set; }
        public string Slug { get; set; }
    }
}