using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "El Correo no puede estar vacio!")]
        [EmailAddress(ErrorMessage = "El Correo debe tener un formato adecuado ejemplo: example@gmail.com")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La Contraseña NO puede estar vacia")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? Role { get; set; }
    }
}
