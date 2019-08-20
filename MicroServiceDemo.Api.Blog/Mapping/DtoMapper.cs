using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Abstractions;
using MicroServiceDemo.Api.Blog.Data;
using MicroServiceDemo.Api.Blog.Data.Entities;
using MicroServiceDemo.Api.Blog.Models;
using MicroServiceDemo.Api.Blog.Security;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceDemo.Api.Blog.Mapping
{
    /// <inheritdoc />
    public class DtoMapper : IDtoMapper
    {
        private readonly BlogDbContext _context;
        private readonly UserPrincipalAccessor _currentUserAccessor;

        /// <summary>
        /// Instantiate a mapper class
        /// </summary>
        /// <param name="context">The current database context</param>
        /// <param name="currentUserAccessor">The currently logged in user</param>
        public DtoMapper(BlogDbContext context, UserPrincipalAccessor currentUserAccessor)
        {
            _context = context;
            _currentUserAccessor = currentUserAccessor;
        }
        
        /// <inheritdoc />
        public async Task<Article> MapArticleAsync(ArticleDto src, Article dst, CancellationToken cancellationToken = new CancellationToken())
        {
            dst.Slug = src.Slug;
            dst.Author = await MapUserAsync(src.Author, dst.Author, cancellationToken);
            dst.AuditRecord = MapAuditRecord(src, dst.AuditRecord);
            dst.ArticleTags = await MapTagsAsync(src.TagList.ToHashSet(), dst.ArticleTags, cancellationToken);
            dst.Body = src.Body;
            dst.AuthorId = dst.Author.Id;
            dst.Description = src.Description;
            dst.Title = src.Title;

            return dst;
        }
        
        /// <inheritdoc />
        public async Task<ICollection<ArticleTags>> MapTagsAsync(HashSet<string> src, ICollection<ArticleTags> dst, CancellationToken cancellationToken = new CancellationToken())
        {
            if(dst == null)
                dst = new List<ArticleTags>();

            //remove tags that are no longer valid
            foreach (var tag in dst.ToList().Where(tag => !src.Remove(tag.Tag.Name)))
            {
                dst.Remove(tag);
            }

            //find existing tags
            foreach(var existingTag in await _context.Tags
                .Where(t => src.Contains(t.Name))
                .ToListAsync(cancellationToken))
            {
                dst.Add(new ArticleTags
                {
                    Tag = existingTag
                });

                src.Remove(existingTag.Name);
            }

            //add tags that don't exist yet
            foreach (var newTag in src)
            {
                dst.Add(new ArticleTags
                {
                    Tag = new Tag
                    {
                        Name = newTag
                    }
                });
            }

            return dst;
        }
        
        /// <inheritdoc />
        public AuditRecord MapAuditRecord(IAuditableDto src, AuditRecord dst)
        {
            if(dst == null)
                dst = new AuditRecord();

            dst.CreatedAt = src.CreatedAt;
            dst.UpdatedAt = src.UpdatedAt;

            return dst;
        }
        
        /// <inheritdoc />
        public async Task<User> MapUserAsync(UserDto src, User dst, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _context.Users
                .Where(u => u.Username == src.Username)
                .SingleAsync(cancellationToken);
        }
        
        /// <inheritdoc />
        public async Task<ArticleDto> MapArticleAsync(Article article, CancellationToken cancellationToken = new CancellationToken())
        {
            var currentUser = await _currentUserAccessor.CurrentUser(cancellationToken);
            var articleDto = new ArticleDto
            {
                Author = await MapUserAsync(article.Author, cancellationToken),
                Slug = article.Slug,
                TagList = MapTags(article.ArticleTags.Select(at => at.Tag)),
                Body = article.Body,
                Description = article.Description,
                FavoritesCount = article.FavoritesCount,
                Title = article.Title,
                Favorited = currentUser?.FavoriteArticles?.Contains(article.Id)??false
            };

            MapAuditRecord(article.AuditRecord, articleDto);

            return articleDto;
        }
        
        /// <inheritdoc />
        public void MapAuditRecord(AuditRecord src, IAuditableDto dst)
        {
            dst.CreatedAt = src.CreatedAt;
            dst.UpdatedAt = src.UpdatedAt;
        }
        
        /// <inheritdoc />
        public string[] MapTags(IEnumerable<Tag> tags)
        {
            return tags.Select(t => t.Name).ToArray();
        }
        
        /// <inheritdoc />
        public async Task<UserDto> MapUserAsync(User user, CancellationToken cancellationToken = new CancellationToken())
        {
            var currentUser = await _currentUserAccessor.CurrentUser(cancellationToken);
            return new UserDto
            {
                Username = user.Username,
                Bio = user.Bio,
                Image = user.Image,
                Following = currentUser?
                    .FollowedUsers?
                    .Contains(user.Id)
                        ??false
            };
        }
    }
}
