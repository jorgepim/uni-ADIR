namespace AppClinica.Models
{
    public class OpcionRespuesta
    {
        public int IdOpcion { get; set; }
        public int Codigo { get; set; }
        public string Descripcion { get; set; }

        public int IdPregunta { get; set; }
        public Pregunta Pregunta { get; set; }
    }
}
