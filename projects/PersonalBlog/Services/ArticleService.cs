using Microsoft.EntityFrameworkCore;
using PersonalBlog.Models;
using PersonalBlog.Models.DTOs;
using PersonalBlog.Persistence;

namespace PersonalBlog.Services;

public class ArticleService : IArticleService
{

    private readonly ApplicationDbContext _dbContext;
    
    public ArticleService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ArticleResponse> AddArticle(ArticleRequest article)
    {
        ValidationHelper.Validate(article);
        
        var newArticle = article.ToArticle();
        newArticle.CreatedAt = DateTime.UtcNow;
        newArticle.LastUpdatedAt = DateTime.UtcNow;

        _dbContext.Article.Add(newArticle);
        await _dbContext.SaveChangesAsync();
        return newArticle.ToArticleResponse();
    }

    public async Task<ArticleResponse> UpdateArticle(ArticleUpdateRequest article)
    {
        ValidationHelper.Validate(article);

        Article? existingArticle = await _dbContext.Article.FirstOrDefaultAsync(a => a.ArticleId == article.ArticleId);
        if (existingArticle is null)
            throw new KeyNotFoundException($"Article with id {article.ArticleId} not found");

        existingArticle.Title = article.Title;
        existingArticle.Content = article.Content;
        existingArticle.LastUpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return existingArticle.ToArticleResponse();
    }

    public async Task<bool> DeleteArticle(int articleId)
    {
        Article? article = await _dbContext.Article.FirstOrDefaultAsync(a => a.ArticleId == articleId);
        if (article is null)
            return false;

        _dbContext.Article.Remove(article);
        int deletedRows = await _dbContext.SaveChangesAsync();
        return deletedRows > 0;
    }

    public async Task<ArticleResponse> GetArticle(int articleId)
    {
        if (articleId <= 0)
            throw new ArgumentException("Article ID must be greater than 0", nameof(articleId));

        Article? article = await _dbContext.Article.FirstOrDefaultAsync(a => a.ArticleId == articleId);
        return article is null
            ? throw new KeyNotFoundException($"Article with id {articleId} not found")
            : article.ToArticleResponse();
    }

    public async Task<IEnumerable<ArticleResponse>> GetArticles()
    {
        return await _dbContext.Article.Select(a => a.ToArticleResponse()).ToListAsync();
    }
}