using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace ExpenseTracker.Models.ExpenseTracker
{
    /// <summary>
    /// Representa un gasto en la categoría de Comestibles / Supermercado.
    /// </summary>
    [CollectionName("Groceries")]
    public class Groceries : ExpenseBase
    {
        /// <summary>
        /// Obtiene o establece el nombre de la tienda o supermercado (ej. Walmart, Costco, Mercado Local).
        /// </summary>
        [Required(ErrorMessage = "El nombre de la tienda es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre de la tienda debe tener entre {2} y {1} caracteres.")]
        public string StoreName { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la categoría del producto (ej. Alimentos, Bebidas, Limpieza, Cuidado Personal, Carne, Lácteos).
        /// </summary>
        [Required(ErrorMessage = "La categoría del producto es obligatoria.")]
        [StringLength(50, ErrorMessage = "La categoría del producto no puede exceder los {1} caracteres.")]
        public string GroceryCategory { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la cantidad de artículos comprados.
        /// </summary>
        [Required(ErrorMessage = "La cantidad de artículos es obligatoria.")]
        [Range(1, 10000, ErrorMessage = "La cantidad debe estar entre {1} y {2}.")]
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Obtiene o establece el precio unitario del artículo.
        /// </summary>
        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0.")]
        [DataType(DataType.Currency, ErrorMessage = "El precio debe ser un valor monetario válido.")]
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Obtiene o establece el número de recibo o factura si está disponible.
        /// </summary>
        [StringLength(50, ErrorMessage = "El número de recibo no puede exceder los {1} caracteres.")]
        public string? ReceiptNumber { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la compra fue al mayoreo / por volumen.
        /// </summary>
        public bool IsBulkPurchase { get; set; }

        /// <summary>
        /// Obtiene o establece el peso o volumen del producto (ej. "2 kg", "1 L").
        /// </summary>
        [StringLength(50, ErrorMessage = "El peso o volumen no puede exceder los {1} caracteres.")]
        public string? WeightOrVolume { get; set; }

        /// <summary>
        /// Obtiene o establece la marca del producto.
        /// </summary>
        [StringLength(100, ErrorMessage = "La marca no puede exceder los {1} caracteres.")]
        public string? Brand { get; set; }
    }
}
