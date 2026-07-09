using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace ExpenseTracker.Models.ExpenseTracker
{
    /// <summary>
    /// Representa un gasto en la categoría de Salud (consultas, medicamentos, procedimientos, etc.).
    /// </summary>
    [CollectionName("Health")]
    public class Health : ExpenseBase
    {
        /// <summary>
        /// Obtiene o establece el tipo de gasto de salud (ej. Consulta, Medicamento, Procedimiento, Estudio, Cirugía, Seguro).
        /// </summary>
        [Required(ErrorMessage = "El tipo de gasto de salud es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo de gasto de salud no puede exceder los {1} caracteres.")]
        public string HealthType { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre del proveedor (ej. Hospital, Clínica, Farmacia, nombre del Doctor).
        /// </summary>
        [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El proveedor debe tener entre {2} y {1} caracteres.")]
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre del especialista o médico que atendió.
        /// </summary>
        [StringLength(100, ErrorMessage = "El nombre del especialista no puede exceder los {1} caracteres.")]
        public string? SpecialistName { get; set; }

        /// <summary>
        /// Obtiene o establece la especialidad médica (ej. Cardiología, Dermatología, Medicina General).
        /// </summary>
        [StringLength(100, ErrorMessage = "La especialidad no puede exceder los {1} caracteres.")]
        public string? Specialty { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del medicamento recetado (si aplica).
        /// </summary>
        [StringLength(200, ErrorMessage = "El nombre del medicamento no puede exceder los {1} caracteres.")]
        public string? MedicationName { get; set; }

        /// <summary>
        /// Obtiene o establece la dosis del medicamento (ej. "500 mg", "2 tabletas al día").
        /// </summary>
        [StringLength(100, ErrorMessage = "La dosis no puede exceder los {1} caracteres.")]
        public string? Dosage { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador o número de receta.
        /// </summary>
        [StringLength(50, ErrorMessage = "El número de receta no puede exceder los {1} caracteres.")]
        public string? PrescriptionId { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el gasto está cubierto por el seguro.
        /// </summary>
        public bool IsCoveredByInsurance { get; set; }

        /// <summary>
        /// Obtiene o establece el monto reembolsado por el seguro, si aplica.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "El reembolso del seguro no puede ser negativo.")]
        [DataType(DataType.Currency, ErrorMessage = "El reembolso debe ser un valor monetario válido.")]
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal? InsuranceReimbursement { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del paciente (útil para gastos médicos familiares).
        /// </summary>
        [StringLength(100, ErrorMessage = "El nombre del paciente no puede exceder los {1} caracteres.")]
        public string? PatientName { get; set; }
    }
}
