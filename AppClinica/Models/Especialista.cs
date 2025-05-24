using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AppClinica.Models
{
    public class Especialista
    {
        public int IdEspecialista { get; set; }
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Direccion { get; set; }

        [ValidateNever]
        public Usuario? Usuario { get; set; }
        [ValidateNever]
        public ICollection<Paciente>? Pacientes { get; set; }
        [ValidateNever]
        public ICollection<ResultadoTest>? ResultadosTest { get; set; }
    }
}
