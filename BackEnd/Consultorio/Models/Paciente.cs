namespace Consultorio.Models
{
    public class Paciente
    {
        public int IdPaciente { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; } = string.Empty;
        public int IdEspecialista { get; set; }
        public DateTime FechaRegistro { get; set; }

        public Especialista Especialista { get; set; }
        public ICollection<ResultadoTest> ResultadosTest { get; set; }
    }

}
