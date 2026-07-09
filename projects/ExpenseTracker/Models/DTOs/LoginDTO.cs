using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El Correo no puede estar vacio!")]
        [EmailAddress(ErrorMessage = "El Correo debe tener un formato adecuado ejemplo: example@gmail.com")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required(ErrorMessage = "La Contraseña NO puede estar vacia")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether to remember the session.
        /// </summary>
        [Display(Name = "Recordar sesión")]
        public bool RememberMe { get; set; }
    }
}
