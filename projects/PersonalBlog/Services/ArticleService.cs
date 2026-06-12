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
        dbContext = _dbContext;
    }
    
    public async Task<ArticleResponse> AddArticle(ArticleRequest article)
    {
        ValidationHelper.Validate(article);
        
        _dbContext.Article.Add(article.ToArticle());
        await _dbContext.SaveChangesAsync();
        return article.ToArticle().ToArticleResponse();
    }

    public async Task<ArticleResponse> UpdateArticle(ArticleUpdateRequest article)
    {
        
        
        Article? existingArticle = await _dbContext.Article.FirstOrDefaultAsync(a => a.ArticleId == article.ArticleId);
        if (existingArticle == null)
        {
            throw new NullReferenceException($"Article with id {article.ArticleId} not found");
        }
        ValidationHelper.Validate(article);
        
        existingArticle.Title = article.Title;
        existingArticle.Content = article.Content;

        await _dbContext.SaveChangesAsync();

        return existingArticle.ToArticleResponse();
    }

    public async Task<bool> DeleteArticle(int articleId)
    {
        Article? article = await _dbContext.Article.FirstOrDefaultAsync(a => a.ArticleId == articleId);
        
        _dbContext.Article.Remove(article);
        int deletedRows = await _dbContext.SaveChangesAsync();
        return deletedRows > 0;
    }

    public async Task<ArticleResponse> GetArticle(int articleId)
    {
        if (articleId >= 1)
        {
            throw new ArgumentException("Article ID cannot be inferior to 0", nameof(articleId));
        }
        Article? article = await _dbContext.Article.FirstOrDefaultAsync(a => a.ArticleId == articleId);
        return article.ToArticleResponse();
    }

    public async Task<IEnumerable<ArticleResponse>> GetArticles()
    {
        return await _dbContext.Article.Select(a => a.ToArticleResponse()).ToListAsync();
    }
}