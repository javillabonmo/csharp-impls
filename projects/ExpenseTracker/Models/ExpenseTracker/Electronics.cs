using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace ExpenseTracker.Models.ExpenseTracker
{
    /// <summary>
    /// Representa un gasto en la categoría de Electrónicos / Tecnología.
    /// </summary>
    [CollectionName("Electronics")]
    public class Electronics : ExpenseBase
    {
        /// <summary>
        /// Obtiene o establece el tipo de producto electrónico (ej. Teléfono, Laptop, Tablet, Audífonos, Monitor, Teclado, TV).
        /// </summary>
        [Required(ErrorMessage = "El tipo de producto electrónico es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo de producto no puede exceder los {1} caracteres.")]
        public string ProductType { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre de la marca (ej. Apple, Samsung, Sony, Dell).
        /// </summary>
        [Required(ErrorMessage = "La marca del producto es obligatoria.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "La marca debe tener entre {2} y {1} caracteres.")]
        public string Brand { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre o número de modelo.
        /// </summary>
        [Required(ErrorMessage = "El modelo del producto es obligatorio.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El modelo debe tener entre {2} y {1} caracteres.")]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la fecha de expiración de la garantía.
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "La fecha de expiración de garantía debe tener un formato válido.")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? WarrantyExpiryDate { get; set; }

        /// <summary>
        /// Obtiene o establece el número de serie del producto.
        /// </summary>
        [StringLength(100, ErrorMessage = "El número de serie no puede exceder los {1} caracteres.")]
        public string? SerialNumber { get; set; }

        /// <summary>
        /// Obtiene o establece la tienda o minorista donde se compró.
        /// </summary>
        [StringLength(100, ErrorMessage = "El nombre de la tienda no puede exceder los {1} caracteres.")]
        public string? StoreName { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad de artículos comprados.
        /// </summary>
        [Required(ErrorMessage = "La cantidad de artículos es obligatoria.")]
        [Range(1, 1000, ErrorMessage = "La cantidad debe estar entre {1} y {2}.")]
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Obtiene o establece la condición del producto (Nuevo, Reacondicionado, Usado).
        /// </summary>
        [StringLength(30, ErrorMessage = "La condición del producto no puede exceder los {1} caracteres.")]
        public string? Condition { get; set; }

        /// <summary>
        /// Obtiene o establece el color o acabado del producto.
        /// </summary>
        [StringLength(50, ErrorMessage = "El color no puede exceder los {1} caracteres.")]
        public string? Color { get; set; }

        /// <summary>
        /// Obtiene o establece el resumen de especificaciones técnicas (ej. "16GB RAM, 512GB SSD").
        /// </summary>
        [StringLength(500, ErrorMessage = "Las especificaciones no pueden exceder los {1} caracteres.")]
        public string? Specifications { get; set; }

        /// <summary>
        /// Obtiene o establece el año de lanzamiento o fabricación.
        /// </summary>
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre {1} y {2}.")]
        public int? ReleaseYear { get; set; }
    }
}
