namespace AppClinica.Models.ViewModels
{
    public class SeleccionarModuloAdirViewModel
    {
        public int? IdPaciente { get; set; }
        public string NombreEncriptado { get; set; } = string.Empty;
        public string ApellidoEncriptado { get; set; } = string.Empty;
        public List<SeccionTest>? Secciones { get; set; } = new();
    }



}
