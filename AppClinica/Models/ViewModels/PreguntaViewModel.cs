namespace AppClinica.Models.ViewModels
{
    public class PreguntaViewModel
    {
        public int IdPregunta { get; set; }
        public string TextoPregunta { get; set; } = "";
        public int Orden { get; set; }
        public int Puntuacion { get; set; } // Se captura del formulario
        public string? Comentario { get; set; }
    }
}
