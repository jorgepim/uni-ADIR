using System.ComponentModel.DataAnnotations;

namespace AppClinica.Models
{
    public class ComentarioSeccionResultado
    {
        [Key]
        public int IdComentarioSeccion { get; set; }

        public int IdResultado { get; set; }

        public int IdSeccion { get; set; }

        [Required]
        public string Comentario { get; set; } = string.Empty;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public ResultadoTest? ResultadoTest { get; set; }

        public SeccionTest? SeccionTest { get; set; }
    }
}
