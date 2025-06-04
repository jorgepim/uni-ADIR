using System.ComponentModel.DataAnnotations;

namespace AppClinica.Models
{
    public class Paciente
    {
        [Key]
        public int IdPaciente { get; set; }

        public int? IdConsentimiento { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Correo { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [MaxLength(1)]
        public string Sexo { get; set; } = string.Empty;

        [Required]
        public string? Responsable { get; set; } = string.Empty;

        [Required]
        public string? Direccion { get; set; } = string.Empty;  


        public int IdEspecialista { get; set; }

        public DateTime FechaRegistro { get; set; }

        // Relaciones
        public Especialista Especialista { get; set; }
        public Consentimiento Consentimiento { get; set; }

        public ICollection<ResultadoTest> ResultadosTest { get; set; }
    }
}
