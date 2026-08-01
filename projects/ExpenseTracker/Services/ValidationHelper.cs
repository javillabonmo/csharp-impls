using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Exceptions;

namespace ExpenseTracker.Services
{
    public class ValidationHelper
    {
        /// <summary>
        /// Validates the specified object using data annotations.
        /// </summary>
        /// <typeparam name="T">The type of the object to validate.</typeparam>
        /// <param name="obj">The object to validate.</param>
        /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
        internal static void Validate<T>(T obj)
        {
            ValidationContext validationContext = new ValidationContext(obj!);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            if (!isValid)
            {
                var errors = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                throw new ArgumentException(errors)
                    .AddData("Operation", "Validate")
                    .AddData("TargetType", typeof(T).Name)
                    .AddData("ErrorCount", validationResults.Count);
            }
        }
    }
}
