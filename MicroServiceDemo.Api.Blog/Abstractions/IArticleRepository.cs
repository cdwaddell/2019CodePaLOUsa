using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Models;
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
        Task<ActionResult<ArticlesDto>> QueryAsync(PageQueryDto query, CancellationToken cancellationToken);
    }
}