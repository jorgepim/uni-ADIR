using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppClinica.Models.ViewModels
{
    public class EspecialistaViewModel
    {
        [BindRequired]
        public Usuario Usuario { get; set; } = new Usuario();
        [BindRequired]
        public Especialista Especialista { get; set; } = new Especialista();

        public List<string> EspecialidadesDisponibles { get; set; } = new()
    {
        "Psicología Clínica", "Neuropsicología", "Pediatría", "Psiquiatría", "Terapia Ocupacional"
    };
    }

}
