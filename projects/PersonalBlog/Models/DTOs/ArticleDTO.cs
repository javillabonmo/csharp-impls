// <copyright file="ArticleDTO.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Models.DTOs;

/// <summary>
/// Represents a request to create or update an article.
/// </summary>
public class ArticleRequest
{
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    [Required(ErrorMessage = "*El titulo es obligatorio")]
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the content.
    /// </summary>
    [DataType(DataType.Text)]
    [MaxLength(5000, ErrorMessage = "*El contenido no puede exceder los 5000 caracteres")]
    public string? Content { get; set; }

    /// <summary>
    /// Converts this request to an <see cref="Article"/> entity.
    /// </summary>
    /// <returns>An <see cref="Article"/> instance.</returns>
    public Article ToArticle()
    {
        return new Article { Title = this.Title, Content = this.Content };
    }
}

/// <summary>
/// Represents an article response.
/// </summary>
public class ArticleResponse
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the content.
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Gets or sets the created at date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the created by user identifier.
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the last updated at date.
    /// </summary>
    public DateTime LastUpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last updated by user identifier.
    /// </summary>
    public Guid LastUpdatedBy { get; set; }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Id: {this.Id}, Title: {this.Title}, Content: {this.Content}";
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not ArticleResponse articleResponse)
        {
            return false;
        }

        return this.Id == articleResponse.Id; // Creo que las otras propiedades no son necesarias
    }

    /// <summary>
    /// Converts this response to an <see cref="ArticleUpdateRequest"/>.
    /// </summary>
    /// <returns>An <see cref="ArticleUpdateRequest"/> instance.</returns>
    public ArticleUpdateRequest ToUpdateRequest()
    {
        return new ArticleUpdateRequest()
        {
            ArticleId = this.Id,
            Title = this.Title,
            Content = this.Content,
        };
    }
}

/// <summary>
/// Represents a request to update an existing article.
/// </summary>
public class ArticleUpdateRequest : ArticleRequest
{
    /// <summary>
    /// Gets or sets the article identifier.
    /// </summary>
    [Required(ErrorMessage = "Article Id is required")]
    public required int ArticleId { get; set; }
}

/// <summary>
/// Provides extension methods for <see cref="Article"/>.
/// </summary>
public static class ArticleExtensions
{
    /// <summary>
    /// Converts an <see cref="Article"/> to an <see cref="ArticleResponse"/>.
    /// </summary>
    /// <param name="article">The article to convert.</param>
    /// <returns>An <see cref="ArticleResponse"/> instance.</returns>
    public static ArticleResponse ToArticleResponse(this Article article)
    {
        return new ArticleResponse()
        {
            Id = article.ArticleId,
            Title = article.Title,
            Content = article.Content,
            CreatedAt = article.CreatedAt,
            CreatedBy = article.CreatedBy,
            LastUpdatedAt = article.LastUpdatedAt,
            LastUpdatedBy = article.LastUpdatedBy,
        };
    }
}
