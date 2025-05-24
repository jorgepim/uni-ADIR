using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace AppClinica.Models
{
    public class Especialista
    {
        public int IdEspecialista { get; set; }
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^[A-Z0-9]{8,12}$", ErrorMessage = "El número JVPP debe tener entre 8 y 12 caracteres alfanuméricos, sin espacios.")]
        public string JVPP { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;

        [RegularExpression(@"^\d{4}-\d{4}$", ErrorMessage = "El teléfono debe tener el formato 7777-7777")]
        public string Telefono { get; set; } = string.Empty;
        public string? Direccion { get; set; }

        [ValidateNever]
        public Usuario? Usuario { get; set; }
        [ValidateNever]
        public ICollection<Paciente>? Pacientes { get; set; }
        [ValidateNever]
        public ICollection<ResultadoTest>? ResultadosTest { get; set; }
    }
}
