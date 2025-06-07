namespace AppClinica.Models.ViewModels
{
    public class PreguntaConOpcionesViewModel
    {
        public int IdPregunta { get; set; }
        public string TextoPregunta { get; set; }
        public string TipoRespuesta { get; set; }
        public int Orden { get; set; }

        public List<OpcionRespuestaViewModel> Opciones { get; set; }
    }

    public class OpcionRespuestaViewModel
    {
        public int IdOpcion { get; set; }
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
    }
}
