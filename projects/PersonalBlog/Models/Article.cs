namespace PersonalBlog.Models
{
    using System.ComponentModel.DataAnnotations;
    using PersonalBlog.Models.Common;

    /// <summary>
    /// Representa un artículo en el blog, con propiedades para el título, contenido y metadatos de auditoría.
    /// </summary>
    public class Article : AuditableEntityBase
    {
        [Key]
        public int ArticleId { get;set; }

        [Required]
        [Display(Name = "Titulo")]
        [MaxLength(100)]
        required public string Title { get; set; }

        [Display(Name = "Contenido")]
        [MaxLength(5000)]
        public string? Content {get;set;}

    }
}

