namespace AppClinica.Models.ViewModels
{
    public class EvaluarModuloAdirViewModel
    {
        public int IdPaciente { get; set; }
        public int IdSeccion { get; set; }
        public string NombrePaciente { get; set; } = "";
        public string NombreSeccion { get; set; } = "";
        public List<PreguntaRespuestaViewModel> Preguntas { get; set; } // ✅ CORRECTO

    }
}
