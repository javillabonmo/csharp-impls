using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.Models.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "El Correo no puede estar vacio!")]
    [EmailAddress(ErrorMessage = "El Correo debe tener un formato adecuado ejemplo: example@gmail.com")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La Contraseña NO puede estar vacia")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Recordar sesión")]
    public bool RememberMe { get; set; }
}