using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace ExpenseTracker.Models.ExpenseTracker
{
    /// <summary>
    /// Representa un gasto en la categoría de Servicios (agua, luz, gas, internet, etc.).
    /// </summary>
    [CollectionName("Utilities")]
    public class Utilities : ExpenseBase
    {
        /// <summary>
        /// Obtiene o establece el tipo de servicio (ej. Electricidad, Agua, Gas, Internet, Teléfono, Streaming).
        /// </summary>
        [Required(ErrorMessage = "El tipo de servicio es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo de servicio no puede exceder los {1} caracteres.")]
        public string ServiceType { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre del proveedor del servicio (ej. CFE, Telmex, Netflix, AT&T).
        /// </summary>
        [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El proveedor debe tener entre {2} y {1} caracteres.")]
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la descripción del período de facturación (ej. "Enero 2026", "Q1 2026").
        /// </summary>
        [StringLength(50, ErrorMessage = "El período de facturación no puede exceder los {1} caracteres.")]
        public string? BillingPeriod { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de vencimiento para el pago de la factura.
        /// </summary>
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "La fecha de vencimiento debe tener un formato válido.")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha en que se pagó la factura realmente.
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "La fecha de pago debe tener un formato válido.")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? PaidDate { get; set; }

        /// <summary>
        /// Obtiene o establece el número de cuenta o cliente asociado al servicio.
        /// </summary>
        [StringLength(50, ErrorMessage = "El número de cuenta no puede exceder los {1} caracteres.")]
        public string? AccountNumber { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del contrato o suscripción.
        /// </summary>
        [StringLength(50, ErrorMessage = "El identificador de contrato no puede exceder los {1} caracteres.")]
        public string? ContractId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la factura se pagó a tiempo.
        /// </summary>
        public bool PaidOnTime { get; set; }

        /// <summary>
        /// Obtiene o establece el cargo por mora si el pago estaba vencido.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "El cargo por mora no puede ser negativo.")]
        [DataType(DataType.Currency, ErrorMessage = "El cargo por mora debe ser un valor monetario válido.")]
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal? LateFee { get; set; }
    }
}
