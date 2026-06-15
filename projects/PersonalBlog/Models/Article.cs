// <copyright file="Article.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

namespace PersonalBlog.Models
{
    using System.ComponentModel.DataAnnotations;
    using PersonalBlog.Models.Common;

    /// <summary>
    /// Representa un artículo en el blog, con propiedades para el título, contenido y metadatos de auditoría.
    /// </summary>
    public class Article : AuditableEntityBase
    {
        /// <summary>
        /// Gets or sets the article identifier.
        /// </summary>
        [Key]
        public int ArticleId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [Required]
        [Display(Name = "Titulo")]
        [MaxLength(100)]
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        [Display(Name = "Contenido")]
        [MaxLength(5000)]
        public string? Content { get; set; }
    }
}
