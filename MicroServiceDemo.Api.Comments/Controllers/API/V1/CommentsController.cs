using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Data.Entities;
using MicroServiceDemo.Api.Blog.Models;
using MicroServiceDemo.Api.Comments.Abstractions;
using MicroServiceDemo.Api.Comments.Models;
using MicroServicesDemo.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace MicroServiceDemo.Api.Comments.Controllers.API.V1
{
    public class ArticlesController: ApiControllerBase
    {
        private readonly IBlogDbContext _context;

        public ArticlesController(IBlogDbContext context)
        {
            _context = context;
        }

        [HttpGet("{slug}/comments")]
        public async Task<ActionResult<CommentsContainerDto>> PosGett(
            [FromRoute] string slug,
            CancellationToken cancellationToken)
        {
            var comments = await _context.Comments
                .Where(c => c.Slug == slug)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Body = c.Body,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    Author = new UserDto
                    {
                        Username = c.Author.Username,
                        Bio = c.Author.Bio,
                        Image = c.Author.Image
                    }
                })
                .ToArrayAsync(cancellationToken);

            return new CommentsContainerDto
            {
                Comments = comments
            };
        }

        [HttpDelete("{slug}/comments/{id}")]
        public async Task<ActionResult<object>> Delete(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var comment = await _context.Comments.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok(new object());
        }

        [HttpPost("{slug}/comments")]
        public async Task<ActionResult<CommentContainerDto>> Post(
            [FromRoute] string slug,
            [FromBody] CommentContainerDto comment,
            CancellationToken cancellationToken)
        {
            var currentUser = User.FindFirst("sub")?.Value;
            if (currentUser == null)
                return Forbid();
            
            //Oh no the user might not exist here yet if they are fast, or we are having an issue
            var user = _context.Users.Single(u => u.Username == currentUser);
            var newComment = new Comment
            {
                AuthorId = user.Id,
                Body = comment.Comment.Body,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _context.Comments.AddAsync(
                newComment
            );

            await _context.SaveChangesAsync(cancellationToken);

            comment.Comment.Id = newComment.Id;
            return comment;
        }
    }
}
