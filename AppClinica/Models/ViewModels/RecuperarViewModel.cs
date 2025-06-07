using System.ComponentModel.DataAnnotations;

namespace AppClinica.Models.ViewModels
{
    public class RecuperarViewModel
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; }
    }

    public class RestablecerViewModel
    {
        public string Correo { get; set; }
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*[^A-Za-z\d]).{6,}$",
    ErrorMessage = "Debe tener al menos 6 caracteres, una letra y un carácter especial.")]
        public string NuevaContrasena { get; set; }

        [Required]
        [Compare("NuevaContrasena")]
        [DataType(DataType.Password)]
        public string ConfirmarContrasena { get; set; }
    }
}
