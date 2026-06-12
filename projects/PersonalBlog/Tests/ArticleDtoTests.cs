using PersonalBlog.Models.DTOs;
using Xunit;

namespace PersonalBlog.Tests;

public class ArticleDtoTests
{
    [Fact]
    public void ArticleRequest_ToArticle_ShouldMapCorrectly()
    {
        var request = new ArticleRequest
        {
            Title = "Test Title",
            Content = "Test Content"
        };

        var result = request.ToArticle();

        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Content, result.Content);
    }

    [Fact]
    public void ArticleRequest_ToArticle_WithNullContent_ShouldMapCorrectly()
    {
        var request = new ArticleRequest
        {
            Title = "Test Title",
            Content = null
        };

        var result = request.ToArticle();

        Assert.Equal("Test Title", result.Title);
        Assert.Null(result.Content);
    }

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
            LastUpdatedBy = Guid.NewGuid()
        };

        var response2 = new ArticleResponse
        {
            Id = 1,
            Title = "Title B",
            Content = "Content B",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(),
            LastUpdatedAt = DateTime.UtcNow,
            LastUpdatedBy = Guid.NewGuid()
        };

        Assert.True(response1.Equals(response2));
    }

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
            LastUpdatedBy = Guid.NewGuid()
        };

        var response2 = new ArticleResponse
        {
            Id = 2,
            Title = "Title",
            Content = "Content",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(),
            LastUpdatedAt = DateTime.UtcNow,
            LastUpdatedBy = Guid.NewGuid()
        };

        Assert.False(response1.Equals(response2));
    }

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
            LastUpdatedBy = Guid.NewGuid()
        };

        Assert.False(response.Equals(null));
    }

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
            LastUpdatedBy = Guid.NewGuid()
        };

        var result = response.ToString();

        Assert.Contains("Id: 1", result);
        Assert.Contains("Title: Test Title", result);
        Assert.Contains("Content: Test Content", result);
    }

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
            LastUpdatedBy = Guid.NewGuid()
        };

        var result = response.ToUpdateRequest();

        Assert.Equal(5, result.ArticleId);
        Assert.Equal("Original Title", result.Title);
        Assert.Equal("Original Content", result.Content);
    }

    [Fact]
    public void ArticleUpdateRequest_Should_HaveArticleId()
    {
        var updateRequest = new ArticleUpdateRequest
        {
            ArticleId = 10,
            Title = "Updated Title",
            Content = "Updated Content"
        };

        Assert.Equal(10, updateRequest.ArticleId);
        Assert.Equal("Updated Title", updateRequest.Title);
        Assert.Equal("Updated Content", updateRequest.Content);
    }
}
