using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace ExpenseTracker.Models.ExpenseTracker
{
    /// <summary>
    /// Representa un gasto en la categoría de Ocio / Entretenimiento.
    /// </summary>
    [CollectionName("Leisure")]
    public class Leisure : ExpenseBase
    {
        /// <summary>
        /// Obtiene o establece el tipo de actividad de ocio (ej. Cena, Cine, Viaje, Pasatiempo, Deportes, Concierto, Gaming).
        /// </summary>
        [Required(ErrorMessage = "El tipo de actividad de ocio es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo de actividad no puede exceder los {1} caracteres.")]
        public string ActivityType { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el nombre del lugar o sede (ej. nombre del restaurante, cine, parque, estadio).
        /// </summary>
        [Required(ErrorMessage = "El lugar o sede es obligatorio.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "El lugar debe tener entre {2} y {1} caracteres.")]
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el número de participantes o personas cubiertas por este gasto.
        /// </summary>
        [Required(ErrorMessage = "El número de participantes es obligatorio.")]
        [Range(1, 1000, ErrorMessage = "El número de participantes debe estar entre {1} y {2}.")]
        public int Participants { get; set; } = 1;

        /// <summary>
        /// Obtiene o establece un valor que indica si el gasto incluyó comida o bebidas.
        /// </summary>
        public bool IncludesFood { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del evento o reservación (ej. "Concierto de Verano", "Cena de Cumpleaños").
        /// </summary>
        [StringLength(200, ErrorMessage = "El nombre del evento no puede exceder los {1} caracteres.")]
        public string? EventName { get; set; }

        /// <summary>
        /// Obtiene o establece el número de confirmación de la reservación o boleto.
        /// </summary>
        [StringLength(50, ErrorMessage = "El número de confirmación no puede exceder los {1} caracteres.")]
        public string? ConfirmationNumber { get; set; }

        /// <summary>
        /// Obtiene o establece la duración de la actividad en horas.
        /// </summary>
        [Range(0.1, 720, ErrorMessage = "La duración debe estar entre {1} y {2} horas.")]
        public double? DurationHours { get; set; }

        /// <summary>
        /// Obtiene o establece el costo de transporte asociado a esta actividad de ocio.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "El costo de transporte no puede ser negativo.")]
        [DataType(DataType.Currency, ErrorMessage = "El costo de transporte debe ser un valor monetario válido.")]
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal? TransportationCost { get; set; }

        /// <summary>
        /// Obtiene o establece la calificación o nivel de satisfacción (1-5).
        /// </summary>
        [Range(1, 5, ErrorMessage = "La calificación de satisfacción debe estar entre {1} y {2}.")]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int? SatisfactionRating { get; set; }
    }
}
