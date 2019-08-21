using MicroServiceDemo.Api.Auth.Abstractions;
using MicroServiceDemo.Api.Auth.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceDemo.Api.Auth.Data
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
        /// Table housing all users
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Entity framework scaffolding method 
        /// </summary>
        /// <param name="modelBuilder">The entity framework model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("auth");

            modelBuilder.Entity<User>(t =>
            {
                t.HasAlternateKey(x => x.Username);
                t.HasAlternateKey(x => x.Email);

                t.Property(x => x.Email)
                    .HasMaxLength(1024);

                t.Property(x => x.Image)
                    .HasMaxLength(1024);

                t.Property(x => x.PasswordHash)
                    .HasMaxLength(1024);

                t.Property(x => x.Salt)
                    .HasMaxLength(1024);

                t.Property(x => x.Username)
                    .HasMaxLength(128);
            });
        }
    }
}
