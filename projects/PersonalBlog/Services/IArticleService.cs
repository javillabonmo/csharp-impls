// <copyright file="IArticleService.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

using PersonalBlog.Models;
using PersonalBlog.Models.DTOs;

namespace PersonalBlog.Services;

/// <summary>
/// Defines the contract for article management operations.
/// </summary>
public interface IArticleService
{
    /// <summary>
    /// Adds a new article.
    /// </summary>
    /// <param name="article">The article request data.</param>
    /// <returns>The created article response.</returns>
    Task<ArticleResponse> AddArticle(ArticleRequest article);

    /// <summary>
    /// Updates an existing article.
    /// </summary>
    /// <param name="article">The article update request data.</param>
    /// <returns>The updated article response.</returns>
    Task<ArticleResponse> UpdateArticle(ArticleUpdateRequest article);

    /// <summary>
    /// Deletes an article by its identifier.
    /// </summary>
    /// <param name="articleId">The article identifier.</param>
    /// <returns>True if the article was deleted, false otherwise.</returns>
    Task<bool> DeleteArticle(int articleId);

    /// <summary>
    /// Gets an article by its identifier.
    /// </summary>
    /// <param name="articleId">The article identifier.</param>
    /// <returns>The article response.</returns>
    Task<ArticleResponse> GetArticle(int articleId);

    /// <summary>
    /// Gets all articles.
    /// </summary>
    /// <returns>A collection of article responses.</returns>
    Task<IEnumerable<ArticleResponse>> GetArticles();
}
