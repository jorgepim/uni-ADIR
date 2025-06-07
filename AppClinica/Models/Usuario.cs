using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AppClinica.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        public int? IdConsentimiento { get; set; }

        public string? NombreUsuario { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string? Correo { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?""{}|<>]).{8,}$",
      ErrorMessage = "Debe tener al menos una letra, un número y un símbolo.")]
        public string? Contrasena { get; set; }

        public int RolId { get; set; }

        public bool? Estado { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public Rol? Rol { get; set; }

        public String? TokenRecuperacion { get; set; }

        //public DateTime? TokenFechaExpiracion { get; set; }

    public Consentimiento? Consentimiento { get; set; }

        public Especialista? Especialista { get; set; }
    }
}
