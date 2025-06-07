using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace AppClinica.Models
{
    public class Especialista
    {
        public int IdEspecialista { get; set; }

        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(100, ErrorMessage = "Los nombres no pueden exceder 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Los nombres solo pueden contener letras y espacios")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden exceder 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Los apellidos solo pueden contener letras y espacios")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número JVPP es obligatorio")]
        [RegularExpression(@"^JVPP-\d{4}\/\d{4}$", ErrorMessage = "El formato del JVPP debe ser: JVPP-1234/2021")]
        [Display(Name = "Número JVPP")]
        public string JVPP { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especialidad es obligatoria")]
        [StringLength(50, ErrorMessage = "La especialidad no puede exceder 50 caracteres")]
        public string Especialidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [RegularExpression(@"^\d{4}-\d{4}$", ErrorMessage = "El teléfono debe tener el formato: 2345-4322")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = string.Empty;

        [ValidateNever]
        public Usuario? Usuario { get; set; }

        [ValidateNever]
        public ICollection<Paciente>? Pacientes { get; set; }

        [ValidateNever]
        public ICollection<ResultadoTest>? ResultadosTest { get; set; }
    }
}