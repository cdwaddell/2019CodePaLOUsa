using MicroServiceDemo.Api.Comments.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceDemo.Api.Comments.Data
{
    /// <summary>
    /// Entity framework database context for blog data
    /// </summary>
    public class BlogDbContext:DbContext, IBlogDbContext
    {
        /// <summary>
        /// Initialize a new entity framework blog database context
        /// </summary>
        /// <param name="options">The options that entity framework uses to construct the context</param>
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Entity framework scaffolding method 
        /// </summary>
        /// <param name="modelBuilder">The entity framework model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("comment");
        }
    }
}
