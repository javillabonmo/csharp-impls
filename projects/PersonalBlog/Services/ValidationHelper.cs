// <copyright file="ValidationHelper.cs" company="TBRZCom">
// Copyright (c) TBRZCom. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Services;

/// <summary>
/// Provides helper methods for object validation using data annotations.
/// </summary>
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

        bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true); // false para validar solo las propiedades requeridas, true para validar todas las propiedades incluso la de las clases que se llaman

        if (!isValid)
        {
            throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
        }
    }
}
