using System.ComponentModel.DataAnnotations;

namespace AppClinica.Models
{
    public class Consentimiento
    {
        [Key]
        public int IdConsentimiento { get; set; }

        public string? Tipo { get; set; }  

        public string? NombreFirmante { get; set; }

        public DateTime FechaConsentimiento { get; set; }

        public string? RutaArchivo { get; set; }

        public bool EnviadoPorCorreo { get; set; }

        public Usuario? Usuario { get; set; }
        public Paciente? Paciente { get; set; }
    }
}
