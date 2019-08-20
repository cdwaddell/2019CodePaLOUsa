using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Data.Entities;
using MicroServiceDemo.Api.Blog.Models;

namespace MicroServiceDemo.Api.Blog.Abstractions
{
    /// <summary>
    /// A mapping interface for mapper cross cutting concerns
    /// </summary>
    public interface IDtoMapper : IMapperBase
    {
        /// <summary>
        /// Map an article API object onto an article EF entity
        /// </summary>
        /// <param name="src">The article API object</param>
        /// <param name="dst">The article EF entity</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Article> MapArticleAsync(ArticleDto src, Article dst, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Map a collection of tags by name onto an EF collection of tag entities
        /// </summary>
        /// <param name="src">A collection of tags</param>
        /// <param name="dst">An EF collection of tag entities</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICollection<ArticleTags>> MapTagsAsync(HashSet<string> src, ICollection<ArticleTags> dst, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Maps an audit interface to and audit EF record
        /// </summary>
        /// <param name="src">The audit API record</param>
        /// <param name="dst">The audit EF entity</param>
        /// <returns></returns>
        AuditRecord MapAuditRecord(IAuditableDto src, AuditRecord dst);
        
        /// <summary>
        /// Map an user API object onto an user EF entity
        /// </summary>
        /// <param name="src">The user API object</param>
        /// <param name="dst">The user EF entity</param>
        /// <param name="cancellationToken"></param>
        Task<User> MapUserAsync(UserDto src, User dst, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Map an article EF object onto an article API entity
        /// </summary>
        /// <param name="src">The article EF object</param>
        /// <param name="cancellationToken"></param>
        Task<ArticleDto> MapArticleAsync(Article src, CancellationToken cancellationToken = new CancellationToken());
        
        /// <summary>
        /// Maps an audit EF record to and audit EF interface
        /// </summary>
        /// <param name="src">The audit EF entity</param>
        /// <param name="dst">The audit API record</param>
        /// <returns></returns>
        void MapAuditRecord(AuditRecord src, IAuditableDto dst);
        
        /// <summary>
        /// Map a tag EF collection onto a tag string collection
        /// </summary>
        /// <param name="src">The tag collection</param>
        string[] MapTags(IEnumerable<Tag> src);

        /// <summary>
        /// Map a user EF entity onto a user API object
        /// </summary>
        /// <param name="src">The user</param>
        /// <param name="cancellationToken"></param>
        Task<UserDto> MapUserAsync(User src, CancellationToken cancellationToken = new CancellationToken());
    }
}