namespace Consultorio.Models
{
    public class SeccionTest
    {
        public int IdSeccion { get; set; }
        public int IdTest { get; set; }
        public string NombreSeccion { get; set; } = string.Empty;
        public int Orden { get; set; }

        public Test Test { get; set; }
        public ICollection<Pregunta> Preguntas { get; set; }
    }

}
