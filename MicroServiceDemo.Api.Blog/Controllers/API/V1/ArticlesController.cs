using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Abstractions;
using MicroServiceDemo.Api.Blog.Models;
using MicroServicesDemo.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiceDemo.Api.Blog.Controllers.API.V1
{
    /// <summary>
    /// Endpoints for viewing and updating articles
    /// </summary>
    [ApiVersion("1.0")]
    public class ArticlesController : ApiControllerBase
    {
        private readonly IArticleRepository _repository;

        /// <summary>
        /// Initializes a new ArticlesController
        /// </summary>
        /// <param name="repository">The article repository</param>
        public ArticlesController(IArticleRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Delete an existing article
        /// </summary>
        /// <param name="slug">The unique key of the article</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{slug}")]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete([FromRoute]string slug, CancellationToken cancellationToken)
        {
             var result = await _repository.DeleteByKeyAsync(slug, cancellationToken);

            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpPost("{slug}/favorite")]
        public async Task<ActionResult<ArticleDto>> AddFavorite([FromRoute] string slug)
        {
            var currentUser = User.FindFirst("sub")?.Value;
            if (currentUser == null)
                return Forbid();

            var article = await _repository.GetByKeyAsync(slug);
            await _repository.AddFavorite(article, currentUser);

            article.Favorited = true;
            return article;
        }

        [HttpDelete("{slug}/favorite")]
        public async Task<ActionResult<ArticleDto>> RemoveFavorite([FromRoute] string slug)
        {
            var currentUser = User.FindFirst("sub")?.Value;
            if (currentUser == null)
                return Forbid();

            var article = await _repository.GetByKeyAsync(slug);
            await _repository.RemoveFavorite(article, currentUser);

            article.Favorited = false;
            return article;
        }

        /// <summary>
        /// Create a new article
        /// </summary>
        /// <param name="article"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The new article</returns>
        [HttpPut]
        public async Task<ActionResult<ArticleDto>> PutNew([FromBody]ArticlePostDto article, CancellationToken cancellationToken)
        {
            return await _repository.AddAsync(article, cancellationToken);
        }

        /// <summary>
        /// Update an article by unique slug
        /// </summary>
        /// <param name="slug">The unique identifier of the article</param>
        /// <param name="article">The updated article</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The updated article</returns>
        [HttpPut("{slug}")]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ArticleDto>> PutUpdate([FromRoute]string slug, [FromBody]ArticlePostDto article, CancellationToken cancellationToken)
        {
            var existingArticle = await _repository.UpdateAsync(slug, article, cancellationToken);
            
            if(existingArticle == null)
                return NotFound();

            return existingArticle;
        }

        /// <summary>
        /// Get a specific article by slug
        /// </summary>
        /// <param name="slug">The unique identifier of the article</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The specified article</returns>
        [AllowAnonymous]
        [HttpGet("{slug}")]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ArticleDto>> GetById([FromRoute]string slug, CancellationToken cancellationToken)
        {
            var article = await _repository.GetByKeyAsync(slug, cancellationToken);

            if (article == null)
                return NotFound();

            return article;
        }

        /// <summary>
        /// Get a list of articles by query parameters
        /// </summary>
        /// <param name="query">The query parameters to use</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ArticlesDto>> Get([FromQuery] PageQueryDto query, CancellationToken cancellationToken)
        {
            return await _repository.QueryAsync(query, cancellationToken);
        }
    }
}