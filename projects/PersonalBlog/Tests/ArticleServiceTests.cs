using Moq;
using PersonalBlog.Models.DTOs;
using PersonalBlog.Services;
using Xunit;

namespace PersonalBlog.Tests;

public class ArticleServiceTests
{
    private readonly Mock<IArticleService> _mockService;
    private readonly List<ArticleResponse> _sampleArticles;

    public ArticleServiceTests()
    {
        _mockService = new Mock<IArticleService>();

        var userId = Guid.NewGuid();
        _sampleArticles =
        [
            new ArticleResponse
            {
                Id = 1,
                Title = "First Article",
                Content = "Content 1",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                CreatedBy = userId,
                LastUpdatedAt = DateTime.UtcNow.AddDays(-2),
                LastUpdatedBy = userId
            },
            new ArticleResponse
            {
                Id = 2,
                Title = "Second Article",
                Content = "Content 2",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                CreatedBy = userId,
                LastUpdatedAt = DateTime.UtcNow.AddDays(-1),
                LastUpdatedBy = userId
            }
        ];
    }

    [Fact]
    public async Task GetArticles_ShouldReturnAllArticles()
    {
        _mockService
            .Setup(s => s.GetArticles())
            .ReturnsAsync(_sampleArticles);

        var result = await _mockService.Object.GetArticles();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetArticle_ExistingId_ShouldReturnArticle()
    {
        var expected = _sampleArticles[0];
        _mockService
            .Setup(s => s.GetArticle(It.IsAny<int>()))
            .ReturnsAsync((int id) => _sampleArticles.FirstOrDefault(a => a.Id == id)!);

        var result = await _mockService.Object.GetArticle(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("First Article", result.Title);
    }

    [Fact]
    public async Task GetArticle_NonExistingId_ShouldReturnNull()
    {
        _mockService
            .Setup(s => s.GetArticle(It.IsAny<int>()))
            .ReturnsAsync((int id) => _sampleArticles.FirstOrDefault(a => a.Id == id)!);

        var result = await _mockService.Object.GetArticle(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddArticle_ShouldReturnCreatedArticle()
    {
        var newArticle = new ArticleRequest
        {
            Title = "New Article",
            Content = "New Content"
        };

        var expectedResponse = new ArticleResponse
        {
            Id = 3,
            Title = "New Article",
            Content = "New Content",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Guid.NewGuid(),
            LastUpdatedAt = DateTime.UtcNow,
            LastUpdatedBy = Guid.NewGuid()
        };

        _mockService
            .Setup(s => s.AddArticle(It.IsAny<ArticleRequest>()))
            .ReturnsAsync(expectedResponse);

        var result = await _mockService.Object.AddArticle(newArticle);

        Assert.NotNull(result);
        Assert.Equal(3, result.Id);
        Assert.Equal("New Article", result.Title);
        Assert.Equal("New Content", result.Content);
    }

    [Fact]
    public async Task AddArticle_NullTitle_ShouldHandleValidation()
    {
        var invalidArticle = new ArticleRequest
        {
            Title = null!,
            Content = "Some content"
        };

        _mockService
            .Setup(s => s.AddArticle(It.Is<ArticleRequest>(r => r.Title == null!)))
            .ThrowsAsync(new ArgumentException("Title is required"));

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _mockService.Object.AddArticle(invalidArticle));

        Assert.Contains("Title", ex.Message);
    }

    [Fact]
    public async Task UpdateArticle_ShouldReturnUpdatedArticle()
    {
        var updateRequest = new ArticleUpdateRequest
        {
            ArticleId = 1,
            Title = "Updated Title",
            Content = "Updated Content"
        };

        var updatedResponse = new ArticleResponse
        {
            Id = 1,
            Title = "Updated Title",
            Content = "Updated Content",
            CreatedAt = DateTime.UtcNow.AddDays(-2),
            CreatedBy = Guid.NewGuid(),
            LastUpdatedAt = DateTime.UtcNow,
            LastUpdatedBy = Guid.NewGuid()
        };

        _mockService
            .Setup(s => s.UpdateArticle(It.IsAny<ArticleUpdateRequest>()))
            .ReturnsAsync(updatedResponse);

        var result = await _mockService.Object.UpdateArticle(updateRequest);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("Updated Content", result.Content);
    }

    [Fact]
    public async Task DeleteArticle_ExistingId_ShouldReturnTrue()
    {
        _mockService
            .Setup(s => s.DeleteArticle(It.IsAny<int>()))
            .ReturnsAsync(true);

        var result = await _mockService.Object.DeleteArticle(1);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteArticle_NonExistingId_ShouldReturnFalse()
    {
        _mockService
            .Setup(s => s.DeleteArticle(It.IsAny<int>()))
            .ReturnsAsync((int id) => _sampleArticles.Any(a => a.Id == id));

        var result = await _mockService.Object.DeleteArticle(99);

        Assert.False(result);
    }

    [Fact]
    public async Task GetArticles_ShouldReturnEmptyList_WhenNoArticles()
    {
        _mockService
            .Setup(s => s.GetArticles())
            .ReturnsAsync(Enumerable.Empty<ArticleResponse>());

        var result = await _mockService.Object.GetArticles();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddArticle_ShouldBeCalledExactlyOnce()
    {
        var article = new ArticleRequest
        {
            Title = "Test",
            Content = "Test Content"
        };

        _mockService
            .Setup(s => s.AddArticle(It.IsAny<ArticleRequest>()))
            .ReturnsAsync(new ArticleResponse
            {
                Id = 1,
                Title = "Test",
                Content = "Test Content",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = Guid.NewGuid(),
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = Guid.NewGuid()
            });

        await _mockService.Object.AddArticle(article);

        _mockService.Verify(s => s.AddArticle(It.IsAny<ArticleRequest>()), Times.Once);
    }
}
