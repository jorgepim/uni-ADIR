using System.ComponentModel.DataAnnotations;

namespace AppClinica.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string Contrasena { get; set; } = string.Empty;

        public int RolId { get; set; }

        public bool Estado { get; set; }

        public DateTime FechaCreacion { get; set; }

        // Relaciones
        public Rol Rol { get; set; }
        public Especialista Especialista { get; set; }
    }
}
