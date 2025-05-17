namespace Consultorio.Models
{
    public class Test
    {
        public int IdTest { get; set; }
        public string NombreTest { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }

        public ICollection<SeccionTest> SeccionesTest { get; set; }
        public ICollection<ResultadoTest> ResultadosTest { get; set; }
    }

}
