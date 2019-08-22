using MicroServiceDemo.Api.Blog.Data.Entities;
using MicroServiceDemo.Api.Comments.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceDemo.Api.Comments.Data
{
    /// <summary>
    /// Entity framework database context for blog data
    /// </summary>
    public class BlogDbContext:DbContext, IBlogDbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Comment> Comments { get; set; }

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

            modelBuilder.Entity<Comment>(t =>
            {
            });
            modelBuilder.Entity<User>(t =>
            {
                t.HasIndex(x => x.Username)
                    .IsUnique();

                t.Property(x => x.Username)
                    .HasMaxLength(128);

                t.Property(x => x.Image)
                    .HasMaxLength(2048);

                t.HasMany(x => x.Comments)
                    .WithOne(x => x.Author)
                    .HasForeignKey(x => x.Id);
            });
        }
    }
}
