using PersonalBlog.Models;
using PersonalBlog.Models.DTOs;

namespace PersonalBlog.Services;

public interface IArticleService
{
    Task<ArticleResponse> AddArticle(ArticleRequest article);
    Task<ArticleResponse> UpdateArticle(ArticleUpdateRequest article); 
    Task<bool> DeleteArticle(int articleId);
    Task<ArticleResponse> GetArticle(int articleId);
    Task<IEnumerable<ArticleResponse>> GetArticles();
}