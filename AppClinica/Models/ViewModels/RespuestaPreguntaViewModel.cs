namespace AppClinica.Models.ViewModels
{
    public class RespuestaPreguntaViewModel
    {
        public int IdPregunta { get; set; }
        public int IdOpcionSeleccionada { get; set; }
    }

    public class ResumenRespuestaViewModel
    {
        public string TextoPregunta { get; set; }
        public string OpcionSeleccionada { get; set; }
        public int CodigoSeleccionado { get; set; }
    }
}
