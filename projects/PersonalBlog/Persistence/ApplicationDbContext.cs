using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.Models;
using PersonalBlog.Models.Auth;
namespace PersonalBlog.Persistence;

public class ApplicationDbContext :  IdentityDbContext<User, Role, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Article> Article { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Article>().Property(p=>p.ArticleId).ValueGeneratedOnAdd();
        modelBuilder.Entity<Article>().Property(p=>p.Title).IsRequired();
        modelBuilder.Entity<Article>().ToTable("Articulos");
    }

}