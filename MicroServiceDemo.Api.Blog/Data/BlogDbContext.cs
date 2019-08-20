using MicroServiceDemo.Api.Blog.Abstractions;
using MicroServiceDemo.Api.Blog.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceDemo.Api.Blog.Data
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
        /// Tags associated with blog articles
        /// </summary>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// Users of the blog system
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Articles in the blog
        /// </summary>
        public DbSet<Article> Articles { get; set; }

        /// <summary>
        /// Favorite articles
        /// </summary>
        public DbSet<FavoriteArticle> FavoriteArticles { get; set; }

        /// <summary>
        /// Entity framework scaffolding method 
        /// </summary>
        /// <param name="modelBuilder">The entity framework model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("blog");

            modelBuilder.Entity<User>(t =>
            {
                t.HasIndex(x => x.Username)
                    .IsUnique();

                t.Property(x => x.Username)
                    .HasMaxLength(128);

                t.Property(x => x.Image)
                    .HasMaxLength(2048);

                t.HasMany(x => x.Articles)
                    .WithOne(x => x.Author)
                    .HasForeignKey(x => x.Id);

                t.HasMany(x => x.FavoriteArticles)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                t.HasMany(x => x.FollowingUsers)
                    .WithOne(x => x.FollowedUser)
                    .HasForeignKey(x => x.FollowedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                t.HasMany(x => x.FollowedByUsers)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Tag>(t =>
            {
                t.HasIndex(x => x.Name)
                    .IsUnique();

                t.Property(x => x.Name)
                    .HasMaxLength(64);

                t.HasMany(x => x.ArticleTags)
                    .WithOne(x => x.Tag)
                    .HasForeignKey(x => x.TagId);
            });
            modelBuilder.Entity<Article>(t =>
            {
                t.HasIndex(x => x.Slug)
                    .IsUnique();

                t.Property(x => x.Slug)
                    .HasMaxLength(64);

                t.Property(x => x.Title)
                    .HasMaxLength(256);

                t.HasMany(x => x.ArticleTags)
                    .WithOne(x => x.Article)
                    .HasForeignKey(x => x.ArticleId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                t.HasMany(x => x.FavoredBy)
                    .WithOne(x => x.Article)
                    .HasForeignKey(x => x.ArticleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
