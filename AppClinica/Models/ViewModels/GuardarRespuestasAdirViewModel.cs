namespace AppClinica.Models.ViewModels
{
    public class GuardarRespuestasAdirViewModel
    {
        public int IdPaciente { get; set; }
        public int IdSeccion { get; set; }

        // clave: IdPregunta, valor: puntuación (0, 1, 2, 3, 8)
        public Dictionary<int, int> Respuestas { get; set; } = new();

        // puedes incluir un comentario general si deseas
        public string? ComentarioGeneral { get; set; }
    }
}
