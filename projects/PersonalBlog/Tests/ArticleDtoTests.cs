// <copyright file="ArticleDtoTests.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

using PersonalBlog.Models.DTOs;
using Xunit;

namespace PersonalBlog.Tests;

/// <summary>
/// Unit tests for <see cref="ArticleRequest"/>, <see cref="ArticleResponse"/>, and <see cref="ArticleUpdateRequest"/> DTOs.
/// </summary>
public class ArticleDtoTests
{
    /// <summary>
    /// Tests that <see cref="ArticleRequest.ToArticle"/> correctly maps all properties.
    /// </summary>
    [Fact]
    public void ArticleRequest_ToArticle_ShouldMapCorrectly()
    {
        var request = new ArticleRequest
        {
            Title = "Test Title",
            Content = "Test Content",
        };

        var result = request.ToArticle();

        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Content, result.Content);
    }

    /// <summary>
    /// Tests that <see cref="ArticleRequest.ToArticle"/> maps correctly when content is null.
    /// </summary>
    [Fact]
    public void ArticleRequest_ToArticle_WithNullContent_ShouldMapCorrectly()
    {
        var request = new ArticleRequest
        {
            Title = "Test Title",
            Content = null,
        };

        var result = request.ToArticle();

        Assert.Equal("Test Title", result.Title);
        Assert.Null(result.Content);
    }

    /// <summary>
    /// Tests that <see cref="ArticleResponse.Equals(object?)"/> returns true for articles with the same ID.
    /// </summary>
    [Fact]
    public void ArticleResponse_Equals_SameId_ShouldReturnTrue()
    {
        var response1 = new ArticleResponse
        {
            Id = 1,
            Title = "Title A",
            Content = "Content A",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(),
            LastUpdatedAt = DateTime.UtcNow,
            LastUpdatedBy = Guid.NewGuid(),
        };

        var response2 = new ArticleResponse
        {
            Id = 1,
            Title = "Title B",
            Content = "Content B",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(),
            LastUpdatedAt = DateTime.UtcNow,
            LastUpdatedBy = Guid.NewGuid(),
        };

        Assert.True(response1.Equals(response2));
    }

    /// <summary>
    /// Tests that <see cref="ArticleResponse.Equals(object?)"/> returns false for articles with different IDs.
    /// </summary>
    [Fact]
    public void ArticleResponse_Equals_DifferentId_ShouldReturnFalse()
    {
        var response1 = new ArticleResponse
        {
            Id = 1,
            Title = "Title",
            Content = "Content",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(),
            LastUpdatedAt = DateTime.UtcNow,
            LastUpdatedBy = Guid.NewGuid(),
        };

        var response2 = new ArticleResponse
        {
            Id = 2,
            Title = "Title",
            Content = "Content",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(),
            LastUpdatedAt = DateTime.UtcNow,
            LastUpdatedBy = Guid.NewGuid(),
        };

        Assert.False(response1.Equals(response2));
    }

    /// <summary>
    /// Tests that <see cref="ArticleResponse.Equals(object?)"/> returns false when comparing to null.
    /// </summary>
    [Fact]
    public void ArticleResponse_Equals_NullObject_ShouldReturnFalse()
    {
        var response = new ArticleResponse
        {
            Id = 1,
            Title = "Title",
            Content = "Content",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(),
            LastUpdatedAt = DateTime.UtcNow,
            LastUpdatedBy = Guid.NewGuid(),
        };

        Assert.False(response.Equals(null));
    }

    /// <summary>
    /// Tests that <see cref="ArticleResponse.ToString()"/> returns the expected formatted string.
    /// </summary>
    [Fact]
    public void ArticleResponse_ToString_ShouldReturnFormattedString()
    {
        var response = new ArticleResponse
        {
            Id = 1,
            Title = "Test Title",
            Content = "Test Content",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(),
            LastUpdatedAt = DateTime.UtcNow,
            LastUpdatedBy = Guid.NewGuid(),
        };

        var result = response.ToString();

        Assert.Contains("Id: 1", result);
        Assert.Contains("Title: Test Title", result);
        Assert.Contains("Content: Test Content", result);
    }

    /// <summary>
    /// Tests that <see cref="ArticleResponse.ToUpdateRequest"/> correctly maps all properties.
    /// </summary>
    [Fact]
    public void ArticleResponse_ToUpdateRequest_ShouldMapCorrectly()
    {
        var response = new ArticleResponse
        {
            Id = 5,
            Title = "Original Title",
            Content = "Original Content",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(),
            LastUpdatedAt = DateTime.UtcNow,
            LastUpdatedBy = Guid.NewGuid(),
        };

        var result = response.ToUpdateRequest();

        Assert.Equal(5, result.ArticleId);
        Assert.Equal("Original Title", result.Title);
        Assert.Equal("Original Content", result.Content);
    }

    /// <summary>
    /// Tests that <see cref="ArticleUpdateRequest"/> correctly stores the article ID.
    /// </summary>
    [Fact]
    public void ArticleUpdateRequest_Should_HaveArticleId()
    {
        var updateRequest = new ArticleUpdateRequest
        {
            ArticleId = 10,
            Title = "Updated Title",
            Content = "Updated Content",
        };

        Assert.Equal(10, updateRequest.ArticleId);
        Assert.Equal("Updated Title", updateRequest.Title);
        Assert.Equal("Updated Content", updateRequest.Content);
    }
}
