using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceDemo.Api.Comments.Abstractions
{
    /// <summary>
    /// Entity framework database context for blog data
    /// </summary>
    public interface IBlogDbContext
    {
        /// <summary>
        /// Save changes to the database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The number of rows affected</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DbSet<User> Users { get; set; }

        DbSet<Comment> Comments { get; set; }

        int SaveChanges();
    }
}