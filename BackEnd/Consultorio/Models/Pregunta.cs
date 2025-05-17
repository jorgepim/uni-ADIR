namespace Consultorio.Models
{
    public class Pregunta
    {
        public int IdPregunta { get; set; }
        public int IdSeccion { get; set; }
        public string TextoPregunta { get; set; } = string.Empty;
        public string TipoRespuesta { get; set; } = string.Empty;
        public int Orden { get; set; }

        public SeccionTest SeccionTest { get; set; }
        public ICollection<RespuestaOpcion> RespuestasOpcion { get; set; }
        public ICollection<RespuestaPaciente> RespuestasPaciente { get; set; }
    }

}
