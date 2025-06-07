namespace AppClinica.Models.ViewModels
{
    public class PreguntasSeccionViewModel
    {
        public int IdPaciente { get; set; }
        public string NombrePaciente { get; set; } = "";
        public SeccionTest? Seccion { get; set; }
    }

}
