using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceDemo.Api.Blog.Abstractions
{
    /// <summary>
    /// Entity framework database context for blog data
    /// </summary>
    public interface IBlogDbContext
    {
        /// <summary>
        /// Tags associated with blog articles
        /// </summary>
        DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// Users of the blog system
        /// </summary>
        DbSet<User> Users { get; set; }

        /// <summary>
        /// Articles in the blog
        /// </summary>
        DbSet<Article> Articles { get; set; }

        /// <summary>
        /// Favorite articles
        /// </summary>
        DbSet<FavoriteArticle> FavoriteArticles { get; set; }

        /// <summary>
        /// Save changes to the database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The number of rows affected</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}