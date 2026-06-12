using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Models.DTOs;

public class ArticleRequest
{        
    [Required(ErrorMessage = "*El titulo es obligatorio")]
    public required string Title {get; set; }
    [DataType(DataType.Text)]
    public string? Content {get;set;}

    public Article ToArticle()
    {
        return new Article { Title = Title, Content = Content };
    }
}

public class ArticleResponse
{
    public int Id {get;set;}
    
    public required string Title {get; set; }
    
    public string? Content {get;set;}
    
    public DateTime CreatedAt { get; set; }
    
    public Guid CreatedBy { get; set; }
    
    public DateTime LastUpdatedAt { get; set; }
    
    public Guid LastUpdatedBy { get; set; }
    
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"Id: {Id}, Title: {Title}, Content: {Content}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ArticleResponse articleResponse)
        {
            return false;
        }

        return Id == articleResponse.Id;//creo que las otras propiedades no son necesarias
    }

    public ArticleUpdateRequest ToUpdateRequest()
    {
        return new ArticleUpdateRequest()
        {
            ArticleId = Id,
            Title = Title,
            Content = Content,
        };
    }
}

public class ArticleUpdateRequest : ArticleRequest
{
    [Required(ErrorMessage = "Article Id is required")]
    public required int ArticleId { get; set; }
    
}
public static class ArticleExtensions
{
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
            LastUpdatedBy = article.LastUpdatedBy
        };
    }
}
