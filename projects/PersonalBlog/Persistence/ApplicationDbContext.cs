// <copyright file="ApplicationDbContext.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

namespace PersonalBlog.Persistence
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PersonalBlog.Models;
    using PersonalBlog.Models.Auth;

    /// <summary>
    /// Application database context for the PersonalBlog application.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the Articles DbSet.
        /// </summary>
        public DbSet<Article> Article { get; set; }

        /// <summary>
        /// Configures the entity models for the database context.
        /// </summary>
        /// <param name="modelBuilder">The model builder instance.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Article>().Property(p => p.ArticleId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Article>().Property(p => p.Title).IsRequired();
            modelBuilder.Entity<Article>().ToTable("Articulos");
        }
    }
}
