using System.ComponentModel.DataAnnotations;
using PersonalBlog.Models.Common;

namespace PersonalBlog.Models
{
    public class Article : AuditableEntityBase
    {
        [Key]
        public int ArticleId {get;set;}
        [Required]
        [Display(Name = "Titulo")]
        [MaxLength(100)]
        public required string Title {get; set; }
        [Display(Name = "Contenido")]
        [MaxLength(5000)]
        public string? Content {get;set;}
    }
}

