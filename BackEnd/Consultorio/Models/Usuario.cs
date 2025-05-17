using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consultorio.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("nombre_usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Column("correo")]
        public string Correo { get; set; } = string.Empty;

        [Column("contrasena")]
        public string Contrasena { get; set; } = string.Empty;

        [Column("rol_id")]
        public int RolId { get; set; }

        [Column("estado")]
        public bool Estado { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        public Rol Rol { get; set; }
        public Especialista Especialista { get; set; }
    }
}
