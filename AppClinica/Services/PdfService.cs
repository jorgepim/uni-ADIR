using AppClinica.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace AppClinica.Services
{
    public interface IPdfService
    {
        Task<string> GenerarActaPacienteAsync(Paciente paciente, Consentimiento consentimiento);
        Task<string> GenerarActaEspecialistaAsync(Usuario usuario, Consentimiento consentimiento);
    }

    public class PdfService : IPdfService
    {
        public async Task<string> GenerarActaPacienteAsync(Paciente paciente, Consentimiento consentimiento)
        {
            string path = Path.Combine("App_Data/Consentimientos", $"Paciente_{paciente.IdPaciente}_Acta.pdf");

            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            var doc = new PdfDocument();
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 12);

            gfx.DrawString("Acta de Consentimiento del Paciente", font, XBrushes.Black, new XRect(0, 20, page.Width, 30), XStringFormats.TopCenter);
            gfx.DrawString($"Paciente: {paciente.Nombres} {paciente.Apellidos}", font, XBrushes.Black, 40, 80);
            gfx.DrawString($"Firmante: {consentimiento.NombreFirmante}", font, XBrushes.Black, 40, 100);
            gfx.DrawString($"Fecha: {consentimiento.FechaConsentimiento.ToShortDateString()}", font, XBrushes.Black, 40, 120);
            gfx.DrawString("Consentimiento otorgado para el tratamiento de datos clínicos bajo normas ISO/IEC 27701 y Ley de Protección de Datos Personales.", font, XBrushes.Black, new XRect(40, 150, page.Width - 80, page.Height - 200));

            doc.Save(path);
            doc.Close();

            return path;
        }

        public async Task<string> GenerarActaEspecialistaAsync(Usuario usuario, Consentimiento consentimiento)
        {
            string path = Path.Combine("App_Data/Consentimientos", $"Especialista_{usuario.IdUsuario}_Acta.pdf");

            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            var doc = new PdfDocument();
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 12);

            gfx.DrawString("Acta de Consentimiento del Especialista", font, XBrushes.Black,
                new XRect(0, 20, page.Width, 30), XStringFormats.TopCenter);

            gfx.DrawString($"Especialista: {usuario.NombreUsuario}", font, XBrushes.Black, 40, 80);
            gfx.DrawString($"Firmante: {consentimiento.NombreFirmante}", font, XBrushes.Black, 40, 100);
            gfx.DrawString($"Fecha: {consentimiento.FechaConsentimiento.ToShortDateString()}", font, XBrushes.Black, 40, 120);

            gfx.DrawString(
                "El especialista acepta proteger la confidencialidad de los datos clínicos a los que tendrá acceso.",
                font,
                XBrushes.Black,
                new XRect(40, 150, page.Width - 80, 100),
                XStringFormats.TopLeft);

            doc.Save(path);
            doc.Close();

            return path;
        }

    }
}
