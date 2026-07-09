using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace ExpenseTracker.Models.ExpenseTracker
{
    /// <summary>
    /// Representa un gasto en la categoría de Ropa / Vestimenta.
    /// </summary>
    [CollectionName("Clothing")]
    public class Clothing : ExpenseBase
    {
        /// <summary>
        /// Obtiene o establece el tipo de prenda de vestir (ej. Camisa, Pantalón, Zapatos, Vestido, Chaqueta, Accesorios).
        /// </summary>
        [Required(ErrorMessage = "El tipo de prenda es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo de prenda no puede exceder los {1} caracteres.")]
        public string ItemType { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la talla de la prenda (ej. "M", "L", "32", "8", "Talla Única").
        /// </summary>
        [Required(ErrorMessage = "La talla de la prenda es obligatoria.")]
        [StringLength(20, ErrorMessage = "La talla no puede exceder los {1} caracteres.")]
        public string Size { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre de la marca.
        /// </summary>
        [StringLength(100, ErrorMessage = "La marca no puede exceder los {1} caracteres.")]
        public string? Brand { get; set; }

        /// <summary>
        /// Obtiene o establece el color o estampado de la prenda.
        /// </summary>
        [StringLength(50, ErrorMessage = "El color no puede exceder los {1} caracteres.")]
        public string? Color { get; set; }

        /// <summary>
        /// Obtiene o establece la composición del material (ej. "Algodón", "Poliéster", "Cuero").
        /// </summary>
        [StringLength(100, ErrorMessage = "El material no puede exceder los {1} caracteres.")]
        public string? Material { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad de prendas compradas.
        /// </summary>
        [Required(ErrorMessage = "La cantidad de prendas es obligatoria.")]
        [Range(1, 1000, ErrorMessage = "La cantidad debe estar entre {1} y {2}.")]
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Obtiene o establece la temporada para la cual está destinada la prenda (ej. Primavera, Verano, Otoño, Invierno, Todas las temporadas).
        /// </summary>
        [StringLength(30, ErrorMessage = "La temporada no puede exceder los {1} caracteres.")]
        public string? Season { get; set; }

        /// <summary>
        /// Obtiene o establece la categoría de género (ej. Hombre, Mujer, Unisex, Niños).
        /// </summary>
        [StringLength(30, ErrorMessage = "La categoría de género no puede exceder los {1} caracteres.")]
        public string? GenderCategory { get; set; }

        /// <summary>
        /// Obtiene o establece la tienda o sitio web donde se compró la prenda.
        /// </summary>
        [StringLength(100, ErrorMessage = "El nombre de la tienda no puede exceder los {1} caracteres.")]
        public string? StoreName { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la prenda estaba en oferta o descuento.
        /// </summary>
        public bool IsOnSale { get; set; }

        /// <summary>
        /// Obtiene o establece el precio original antes del descuento.
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio original debe ser mayor a 0.")]
        [DataType(DataType.Currency, ErrorMessage = "El precio original debe ser un valor monetario válido.")]
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal? OriginalPrice { get; set; }
    }
}
