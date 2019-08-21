﻿// <auto-generated />
using System;
using MicroServiceDemo.Api.Blog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MicroServiceDemo.Api.Blog.Data.Migrations
{
    [DbContext(typeof(BlogDbContext))]
    partial class BlogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("blog")
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MicroServiceDemo.Api.Blog.Data.Entities.Article", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("AuthorId");

                    b.Property<string>("Body");

                    b.Property<string>("Description");

                    b.Property<int>("FavoritesCount");

                    b.Property<string>("Slug")
                        .HasMaxLength(64);

                    b.Property<string>("Title")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasFilter("[Slug] IS NOT NULL");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("MicroServiceDemo.Api.Blog.Data.Entities.ArticleTags", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ArticleId");

                    b.Property<int>("TagId");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("TagId");

                    b.ToTable("ArticleTags");
                });

            modelBuilder.Entity("MicroServiceDemo.Api.Blog.Data.Entities.FavoriteArticle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ArticleId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("UserId");

                    b.ToTable("FavoriteArticles");
                });

            modelBuilder.Entity("MicroServiceDemo.Api.Blog.Data.Entities.FollowingUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FollowedUserId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("FollowedUserId");

                    b.HasIndex("UserId");

                    b.ToTable("FollowingUsers");
                });

            modelBuilder.Entity("MicroServiceDemo.Api.Blog.Data.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("MicroServiceDemo.Api.Blog.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Bio");

                    b.Property<string>("Image")
                        .HasMaxLength(2048);

                    b.Property<string>("Sub");

                    b.Property<string>("Username")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MicroServiceDemo.Api.Blog.Data.Entities.Article", b =>
                {
                    b.HasOne("MicroServiceDemo.Api.Blog.Data.Entities.User", "Author")
                        .WithMany("Articles")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("MicroServiceDemo.Api.Blog.Data.Entities.AuditRecord", "AuditRecord", b1 =>
                        {
                            b1.Property<int>("ArticleId");

                            b1.Property<DateTime>("CreatedAt");

                            b1.Property<DateTime>("UpdatedAt");

                            b1.HasKey("ArticleId");

                            b1.ToTable("Articles");

                            b1.HasOne("MicroServiceDemo.Api.Blog.Data.Entities.Article")
                                .WithOne("AuditRecord")
                                .HasForeignKey("MicroServiceDemo.Api.Blog.Data.Entities.AuditRecord", "ArticleId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("MicroServiceDemo.Api.Blog.Data.Entities.ArticleTags", b =>
                {
                    b.HasOne("MicroServiceDemo.Api.Blog.Data.Entities.Article", "Article")
                        .WithMany("ArticleTags")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MicroServiceDemo.Api.Blog.Data.Entities.Tag", "Tag")
                        .WithMany("ArticleTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MicroServiceDemo.Api.Blog.Data.Entities.FavoriteArticle", b =>
                {
                    b.HasOne("MicroServiceDemo.Api.Blog.Data.Entities.Article", "Article")
                        .WithMany("FavoredBy")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MicroServiceDemo.Api.Blog.Data.Entities.User", "User")
                        .WithMany("FavoriteArticles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("MicroServiceDemo.Api.Blog.Data.Entities.FollowingUser", b =>
                {
                    b.HasOne("MicroServiceDemo.Api.Blog.Data.Entities.User", "FollowedUser")
                        .WithMany("FollowingUsers")
                        .HasForeignKey("FollowedUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MicroServiceDemo.Api.Blog.Data.Entities.User", "User")
                        .WithMany("FollowedByUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("MicroServiceDemo.Api.Blog.Data.Entities.Tag", b =>
                {
                    b.OwnsOne("MicroServiceDemo.Api.Blog.Data.Entities.AuditRecord", "AuditRecord", b1 =>
                        {
                            b1.Property<int>("TagId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<DateTime>("CreatedAt");

                            b1.Property<DateTime>("UpdatedAt");

                            b1.HasKey("TagId");

                            b1.ToTable("Tags");

                            b1.HasOne("MicroServiceDemo.Api.Blog.Data.Entities.Tag")
                                .WithOne("AuditRecord")
                                .HasForeignKey("MicroServiceDemo.Api.Blog.Data.Entities.AuditRecord", "TagId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
