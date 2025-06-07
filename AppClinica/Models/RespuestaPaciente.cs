namespace AppClinica.Models
{
    public class RespuestaPaciente
    {
        public int IdRespuestaPaciente { get; set; }
        public int IdResultado { get; set; }
        public int IdPregunta { get; set; }
        public int? Puntuacion { get; set; }
        public string? Comentario { get; set; }

        public ResultadoTest? ResultadoTest { get; set; }
        public Pregunta? Pregunta { get; set; }
    }
}
