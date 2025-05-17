namespace Consultorio.Models
{
    public class ResultadoTest
    {
        public int IdResultado { get; set; }
        public int IdTest { get; set; }
        public int IdPaciente { get; set; }
        public int IdEspecialista { get; set; }
        public DateTime FechaRealizacion { get; set; }
        public string? Observaciones { get; set; }

        public Test Test { get; set; }
        public Paciente Paciente { get; set; }
        public Especialista Especialista { get; set; }
        public ICollection<RespuestaPaciente> RespuestasPaciente { get; set; }
    }

}
