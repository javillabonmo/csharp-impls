using System.ComponentModel.DataAnnotations;
using MongoDbGenericRepository.Attributes;

namespace ExpenseTracker.Models.ExpenseTracker
{
    /// <summary>
    /// Representa un gasto en la categoría de Otros (gastos varios / no categorizados).
    /// </summary>
    [CollectionName("Others")]
    public class Others : ExpenseBase
    {
        /// <summary>
        /// Obtiene o establece el nombre de categoría personalizada definida por el usuario (ej. "Regalos", "Mascotas", "Educación").
        /// </summary>
        [Required(ErrorMessage = "La categoría personalizada es obligatoria.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "La categoría personalizada debe tener entre {2} y {1} caracteres.")]
        public string CustomCategory { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el tipo de pago o transacción (ej. Efectivo, Tarjeta de Crédito, Débito, Transferencia, Crypto).
        /// </summary>
        [Required(ErrorMessage = "El tipo de pago es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo de pago no puede exceder los {1} caracteres.")]
        public string PaymentType { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la URL de la imagen del recibo o factura (si se subió).
        /// </summary>
        [Url(ErrorMessage = "La URL de la imagen del recibo no tiene un formato válido.")]
        [StringLength(500, ErrorMessage = "La URL de la imagen no puede exceder los {1} caracteres.")]
        public string? ReceiptImageUrl { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del vendedor o beneficiario.
        /// </summary>
        [StringLength(200, ErrorMessage = "El nombre del vendedor no puede exceder los {1} caracteres.")]
        public string? VendorName { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si este gasto es deducible de impuestos.
        /// </summary>
        public bool IsTaxDeductible { get; set; }

        /// <summary>
        /// Obtiene o establece la categoría de deducción fiscal si aplica.
        /// </summary>
        [StringLength(100, ErrorMessage = "La categoría fiscal no puede exceder los {1} caracteres.")]
        public string? TaxCategory { get; set; }

        /// <summary>
        /// Obtiene o establece el nivel de prioridad del gasto (ej. "Alta", "Media", "Baja").
        /// </summary>
        [StringLength(20, ErrorMessage = "La prioridad no puede exceder los {1} caracteres.")]
        public string? Priority { get; set; }

        /// <summary>
        /// Obtiene o establece el período de garantía o devolución en días (para artículos físicos).
        /// </summary>
        [Range(0, 365, ErrorMessage = "El período de devolución debe estar entre {1} y {2} días.")]
        public int? ReturnPeriodDays { get; set; }
    }
}
