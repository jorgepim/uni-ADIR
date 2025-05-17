namespace Consultorio.Models
{
    public class RespuestaOpcion
    {
        public int IdRespuesta { get; set; }
        public int IdPregunta { get; set; }
        public string TextoRespuesta { get; set; } = string.Empty;
        public int? Valor { get; set; }

        public Pregunta Pregunta { get; set; }
    }

}
