using System;
using MicroServiceDemo.Api.Blog.Models;

namespace MicroServiceDemo.Api.Comments.Models
{
    public class CommentDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Body { get; set; }
        public UserDto Author { get; set; }
    }
}
