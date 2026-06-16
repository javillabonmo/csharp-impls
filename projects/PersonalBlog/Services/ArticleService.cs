// <copyright file="ArticleService.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using PersonalBlog.Models;
using PersonalBlog.Models.DTOs;
using PersonalBlog.Persistence;
using System.Security.Claims;

namespace PersonalBlog.Services;

/// <summary>
/// Provides article management operations.
/// </summary>
public class ArticleService : IArticleService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArticleService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    public ArticleService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        this._dbContext = dbContext;
        this._httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Adds a new article.
    /// </summary>
    /// <param name="article">The article request data.</param>
    /// <returns>The created article response.</returns>
    public async Task<ArticleResponse> AddArticle(ArticleRequest article)
    {
        ValidationHelper.Validate(article);

        var newArticle = article.ToArticle();
        newArticle.CreatedAt = DateTime.UtcNow;
        newArticle.LastUpdatedAt = DateTime.UtcNow;

        //this shit looks sus
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);


        newArticle.CreatedBy = Guid.Parse(userId ?? Guid.Empty.ToString());
        newArticle.LastUpdatedBy = Guid.Parse(userId ?? Guid.Empty.ToString());


        this._dbContext.Article.Add(newArticle);
        await this._dbContext.SaveChangesAsync();
        return newArticle.ToArticleResponse();
    }

    /// <summary>
    /// Updates an existing article.
    /// </summary>
    /// <param name="article">The article update request data.</param>
    /// <returns>The updated article response.</returns>
    public async Task<ArticleResponse> UpdateArticle(ArticleUpdateRequest article)
    {
        ValidationHelper.Validate(article);

        Article? existingArticle = await this._dbContext.Article.FirstOrDefaultAsync(a => a.ArticleId == article.ArticleId);
        if (existingArticle is null)
        {
            throw new KeyNotFoundException($"Article with id {article.ArticleId} not found");
        }

        existingArticle.Title = article.Title;
        existingArticle.Content = article.Content;
        existingArticle.LastUpdatedAt = DateTime.UtcNow;

        await this._dbContext.SaveChangesAsync();

        return existingArticle.ToArticleResponse();
    }

    /// <summary>
    /// Deletes an article by its identifier.
    /// </summary>
    /// <param name="articleId">The article identifier.</param>
    /// <returns>True if the article was deleted, false otherwise.</returns>
    public async Task<bool> DeleteArticle(int articleId)
    {
        Article? article = await this._dbContext.Article.FirstOrDefaultAsync(a => a.ArticleId == articleId);
        if (article is null)
        {
            return false;
        }

        this._dbContext.Article.Remove(article);
        int deletedRows = await this._dbContext.SaveChangesAsync();
        return deletedRows > 0;
    }

    /// <summary>
    /// Gets an article by its identifier.
    /// </summary>
    /// <param name="articleId">The article identifier.</param>
    /// <returns>The article response.</returns>
    public async Task<ArticleResponse> GetArticle(int articleId)
    {
        if (articleId <= 0)
        {
            throw new ArgumentException("Article ID must be greater than 0", nameof(articleId));
        }

        Article? article = await this._dbContext.Article.FirstOrDefaultAsync(a => a.ArticleId == articleId);
        return article is null
            ? throw new KeyNotFoundException($"Article with id {articleId} not found")
            : article.ToArticleResponse();
    }

    /// <summary>
    /// Gets all articles.
    /// </summary>
    /// <returns>A collection of article responses.</returns>
    public async Task<IEnumerable<ArticleResponse>> GetArticles()
    {
        return await this._dbContext.Article.Select(a => a.ToArticleResponse()).ToListAsync();
    }
}
