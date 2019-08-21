using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Abstractions;
using MicroServiceDemo.Api.Blog.Data;
using MicroServiceDemo.Api.Blog.Data.Entities;
using MicroServiceDemo.Api.Blog.Models;
using MicroServiceDemo.Api.Blog.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceDemo.Api.Blog.Repositories
{
    /// <inheritdoc cref="IArticleRepository"/>>
    public class ArticleRepository: IArticleRepository
    {
        private readonly BlogDbContext _context;
        private readonly IDtoMapper _mapper;
        private readonly UserPrincipalAccessor _userAccessor;

        /// <summary>
        /// Initialize a new ArticleRepository
        /// </summary>
        /// <param name="context">The database context</param>
        /// <param name="userAccessor">The user</param>
        /// <param name="mapper">The automapper mapper</param>
        public ArticleRepository(BlogDbContext context, UserPrincipalAccessor userAccessor, IDtoMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        /// <inheritdoc cref="IArticleRepository"/>>
        public async Task AddFavorite(ArticleDto article, string currentUser, CancellationToken cancellationToken = new CancellationToken())
        {
            var currentFavorite = _context.FavoriteArticles
                .SingleOrDefaultAsync(x => 
                x.User.Username == currentUser &&
                x.Article.Slug == article.Slug, 
                cancellationToken);

            if (currentFavorite == null)
            {
                var articleId = (await _context.Articles
                    .Where(x => x.Slug == article.Slug)
                    .SingleAsync(cancellationToken)).Id;

                var userId = (await _context.Users
                    .Where(x => x.Username == currentUser)
                    .SingleAsync(cancellationToken)).Id;

                _context.FavoriteArticles.Add(new FavoriteArticle
                {
                    ArticleId = articleId,
                    UserId = userId
                });

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <inheritdoc cref="IArticleRepository"/>>
        public async Task RemoveFavorite(ArticleDto article, string currentUser, CancellationToken cancellationToken = new CancellationToken())
        {
            var currentFavorite = await _context.FavoriteArticles
                .SingleOrDefaultAsync(x => 
                        x.User.Username == currentUser &&
                        x.Article.Slug == article.Slug, 
                    cancellationToken);

            if (currentFavorite != null)
            {
                _context.FavoriteArticles.Remove(currentFavorite);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        
        /// <inheritdoc cref="IArticleRepository"/>>
        public async Task<bool> DeleteByKeyAsync(string key, CancellationToken cancellationToken = new CancellationToken())
        {
            var article = await _context.Articles.SingleOrDefaultAsync(x => x.Slug == key, cancellationToken);

            if(article == null)
                return false;

            _context.Articles.Remove(article);

            return await _context.SaveChangesAsync(cancellationToken) == 1;
        }

        /// <inheritdoc cref="IArticleRepository"/>>
        public async Task<ArticleDto> GetByKeyAsync(string key, CancellationToken cancellationToken = new CancellationToken())
        {
            var article = await GetEntityByKey(key, cancellationToken);

            if (article == null)
                return null;

            return await _mapper.MapArticleAsync(article, cancellationToken);
        }

        /// <inheritdoc cref="IArticleRepository"/>>
        public async Task<ArticleDto> AddAsync(ArticleDto entity, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await _userAccessor.CurrentUser(cancellationToken);
            entity.Slug = Guid.NewGuid().ToString("N");
            entity.Author = new UserDto
            {
                Username = user.Username
            };
            var article = await _mapper.MapArticleAsync(entity, new Article(), cancellationToken);

            await _context.Articles.AddAsync(article, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return await _mapper.MapArticleAsync(article, cancellationToken);
        }
        
        /// <inheritdoc cref="IArticleRepository"/>>
        public async Task<ArticleDto> AddAsync(ArticlePostDto entity, CancellationToken cancellationToken = new CancellationToken())
        {
            var article = _mapper.MapArticle(entity);
            return await AddAsync(article, cancellationToken);
        }
        
        /// <inheritdoc cref="IArticleRepository"/>>
        public async Task<ArticleDto> UpdateAsync(string key, ArticlePostDto entity, CancellationToken cancellationToken = new CancellationToken())
        {
            var article = _mapper.MapArticle(entity);
            return await UpdateAsync(key, article, cancellationToken);
        }
        
        /// <inheritdoc cref="IArticleRepository"/>>
        public async Task<ArticleDto> UpdateAsync(string key, ArticleDto entity, CancellationToken cancellationToken = new CancellationToken())
        {
            var article = await GetEntityByKey(key, cancellationToken);

            await _mapper.MapArticleAsync(entity, article, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return await _mapper.MapArticleAsync(article, cancellationToken);
        }
        
        /// <inheritdoc cref="IArticleRepository"/>>
        public async Task<ActionResult<ArticlesDto>> QueryAsync(PageQueryDto query, CancellationToken cancellationToken)
        {
            var articles = new ArticlesDto
            {
                Articles = new List<ArticleDto>(),
                ArticlesCount = await _context.Articles
                    .CountAsync(cancellationToken: cancellationToken)
            };

            var articleQuery = CreateQuery()
                .AsNoTracking();

            if (!string.IsNullOrEmpty(query.Tag))
                articleQuery = articleQuery
                    .Where(a =>
                        a.ArticleTags
                            .Any(at => at.Tag.Name == query.Tag)
                    );

            foreach(var article in articleQuery
                .Skip(query.Offset)
                .Take(query.Limit))
            {
                articles.Articles.Add(await _mapper.MapArticleAsync(article, cancellationToken));
            }

            return articles;
        }

        private async Task<Article> GetEntityByKey(string key, CancellationToken cancellationToken = new CancellationToken())
        {
            var article = await CreateQuery()
                .Where(x => x.Slug == key)
                .SingleOrDefaultAsync(cancellationToken);

            return article;
        }

        private IQueryable<Article> CreateQuery()
        {
            return _context.Articles
                .Include(x => x.ArticleTags)
                .ThenInclude(x => x.Tag);
        }
    }
}
