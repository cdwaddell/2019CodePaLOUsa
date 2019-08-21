using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Auth.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceDemo.Api.Auth.Abstractions
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

        /// <summary>
        /// Users assigned to the blog systems
        /// </summary>
        DbSet<User> Users { get; set; }
    }
}