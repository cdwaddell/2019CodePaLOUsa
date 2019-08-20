using System.Threading;
using System.Threading.Tasks;

namespace MicroServiceDemo.Api.Blog.Abstractions
{
    /// <summary>
    /// A Repository for housing <typeparamref name="T"></typeparamref> entities
    /// </summary>
    /// <typeparam name="T">The type of entity this repository houses</typeparam>
    /// <typeparam name="TKey">The type of key to uniquely identity the entity type</typeparam>
    public interface IRepository<T, in TKey>: IRepositoryBase
    {
        /// <summary>
        /// Delete an entity <typeparamref name="T"></typeparamref> by key <typeparamref name="TKey"/>
        /// </summary>
        /// <param name="key">The key to uniquely identity the <typeparamref name="T"></typeparamref> </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteByKeyAsync(TKey key, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Get an entity <typeparamref name="T"></typeparamref> by key <typeparamref name="TKey"/>
        /// </summary>
        /// <param name="key">The key to uniquely identity the <typeparamref name="T"></typeparamref> </param>
        /// <param name="cancellationToken"></param>
        /// <returns>The entity <typeparamref name="T"></typeparamref></returns>
        Task<T> GetByKeyAsync(TKey key, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Add an entity <typeparamref name="T"></typeparamref> 
        /// </summary>
        /// <param name="entity">The entity <typeparamref name="T"></typeparamref> to add</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The added entity <typeparamref name="T"></typeparamref> </returns>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Update an entity <typeparamref name="T"></typeparamref> with key <typeparamref name="TKey"></typeparamref> 
        /// </summary>
        /// <param name="key">The key <typeparamref name="TKey"></typeparamref> to identity the entity to update</param>
        /// <param name="entity">The updated entity <typeparamref name="T"></typeparamref></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> UpdateAsync(TKey key, T entity, CancellationToken cancellationToken = new CancellationToken());
    }
}