namespace AppClinica.Models.ViewModels
{
    public class PreguntaRespuestaViewModel
    {
        public int IdPregunta { get; set; }
        public int Orden { get; set; }
        public string TextoPregunta { get; set; }

        // Propiedades para respuestas del usuario
        public int? Puntuacion { get; set; }
        public string Comentario { get; set; }
    }
}
