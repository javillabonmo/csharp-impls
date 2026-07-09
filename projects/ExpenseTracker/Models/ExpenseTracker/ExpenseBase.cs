using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace ExpenseTracker.Models.ExpenseTracker
{
    /// <summary>
    /// Clase base para todas las categorías de gastos.
    /// Contiene los campos comunes compartidos por todos los tipos de gasto.
    /// </summary>
    [BsonKnownTypes(
        typeof(Groceries),
        typeof(Utilities),
        typeof(Health),
        typeof(Leisure),
        typeof(Clothing),
        typeof(Electronics),
        typeof(Others))]
    public abstract class ExpenseBase
    {
        /// <summary>
        /// Obtiene o establece el identificador único del gasto (ObjectId de MongoDB).
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        /// <summary>
        /// Obtiene o establece el ID del usuario que creó este gasto (FK a AspNetUsers).
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        [JsonIgnore]
        public Guid UserId { get; set; }

        /// <summary>
        /// Obtiene o establece el monto monetario del gasto.
        /// </summary>
        [Required(ErrorMessage = "El monto del gasto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto del gasto debe ser mayor a 0.")]
        [DataType(DataType.Currency, ErrorMessage = "El monto debe ser un valor monetario válido.")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Amount { get; set; }

        /// <summary>
        /// Obtiene o establece una descripción corta del gasto.
        /// </summary>
        [Required(ErrorMessage = "La descripción del gasto es obligatoria.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre {2} y {1} caracteres.")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la fecha en que ocurrió el gasto.
        /// </summary>
        [Required(ErrorMessage = "La fecha del gasto es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "La fecha debe tener un formato válido.")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece el método de pago utilizado (ej. Efectivo, Tarjeta de Crédito, Débito, Transferencia).
        /// </summary>
        [StringLength(50, ErrorMessage = "El método de pago no puede exceder los {1} caracteres.")]
        public string? PaymentMethod { get; set; }

        /// <summary>
        /// Obtiene o establece notas o comentarios adicionales sobre el gasto.
        /// </summary>
        [StringLength(1000, ErrorMessage = "Las notas no pueden exceder los {1} caracteres.")]
        public string? Notes { get; set; }

        /// <summary>
        /// Obtiene o establece la marca de tiempo de creación del registro.
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Obtiene o establece la marca de tiempo de la última actualización del registro.
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el gasto es recurrente (suscripción, factura mensual, etc.).
        /// </summary>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// Obtiene o establece el intervalo de recurrencia para gastos recurrentes (ej. "Mensual", "Anual").
        /// </summary>
        [StringLength(50, ErrorMessage = "El intervalo de recurrencia no puede exceder los {1} caracteres.")]
        public string? RecurrenceInterval { get; set; }

        /// <summary>
        /// Obtiene o establece las etiquetas asociadas a este gasto para categorización y filtrado.
        /// </summary>
        public List<string> Tags { get; set; } = [];
    }
}
