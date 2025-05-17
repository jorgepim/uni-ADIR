namespace Consultorio.Models
{
    public class RespuestaPaciente
    {
        public int IdRespuestaPaciente { get; set; }
        public int IdResultado { get; set; }
        public int IdPregunta { get; set; }
        public string? RespuestaTexto { get; set; }
        public int? IdRespuestaOpcion { get; set; }

        public ResultadoTest ResultadoTest { get; set; }
        public Pregunta Pregunta { get; set; }
        public RespuestaOpcion? RespuestaOpcion { get; set; }
    }

}
