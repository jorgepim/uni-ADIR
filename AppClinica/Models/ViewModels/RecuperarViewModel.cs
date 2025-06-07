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
        public string NuevaContrasena { get; set; }

        [Required]
        [Compare("NuevaContrasena")]
        [DataType(DataType.Password)]
        public string ConfirmarContrasena { get; set; }
    }
}
