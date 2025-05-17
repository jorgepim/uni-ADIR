namespace Consultorio.Models
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

        public Usuario Usuario { get; set; }
        public ICollection<Paciente> Pacientes { get; set; }
        public ICollection<ResultadoTest> ResultadosTest { get; set; }
    }

}
