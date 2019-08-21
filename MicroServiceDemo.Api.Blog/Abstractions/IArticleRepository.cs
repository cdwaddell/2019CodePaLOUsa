using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Models;
using MicroServicesDemo.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceDemo.Api.Blog.Abstractions
{
    /// <inheritdoc cref="IRepository{T,TKey}"/>
    public interface IArticleRepository: IRepository<ArticleDto, string>
    {
        /// <summary>
        /// Query the Article entities
        /// </summary>
        /// <param name="query">The query to perform</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ActionResult<ArticlesDto>> QueryAsync(PageQueryDto query, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Flag an article as favorite for the provided user
        /// </summary>
        /// <param name="article"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task AddFavorite(ArticleDto article, string currentUser, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Remove an article as favorite for the provided user
        /// </summary>
        /// <param name="article"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        Task RemoveFavorite(ArticleDto article, string currentUser, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Update an entity Article with slug string
        /// </summary>
        /// <param name="key">The key slug to identity the entity to update</param>
        /// <param name="entity">The updated entity Article</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ArticleDto> UpdateAsync(string key, ArticlePostDto entity, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Update an entity Article with slug string
        /// </summary>
        /// <param name="entity">The updated entity Article</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ArticleDto> AddAsync(ArticlePostDto entity, CancellationToken cancellationToken = new CancellationToken());
    }
}