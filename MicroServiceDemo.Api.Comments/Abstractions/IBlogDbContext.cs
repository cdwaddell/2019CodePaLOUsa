using System.Threading;
using System.Threading.Tasks;

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
    }
}