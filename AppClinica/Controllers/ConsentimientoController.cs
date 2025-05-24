using AppClinica.Models;
using AppClinica.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppClinica.Controllers
{
    public class ConsentimientoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPdfService _pdfService;
        private readonly IEmailService _emailService;

        public ConsentimientoController(AppDbContext context, IPdfService pdfService, IEmailService emailService)
        {
            _context = context;
            _pdfService = pdfService;
            _emailService = emailService;
        }

        // Mostrar vista de acta paciente
        [HttpGet]
        public async Task<IActionResult> ActaPaciente(int id)
        {
            var paciente = await _context.Pacientes.Include(p => p.Especialista).FirstOrDefaultAsync(p => p.IdPaciente == id);
            if (paciente == null) return NotFound();
            return View(paciente);
        }

        // Mostrar vista de acta especialista
        [HttpGet]
        public async Task<IActionResult> ActaEspecialista(int id)
        {
            var usuario = await _context.Usuarios.Include(u => u.Especialista).FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // Confirmar y generar acta paciente
        [HttpPost]
        public async Task<IActionResult> ConfirmarConsentimientoPaciente(int idPaciente, string nombreFirmante)
        {
            var paciente = await _context.Pacientes.FindAsync(idPaciente);
            if (paciente == null) return NotFound();

            var consentimiento = new Consentimiento
            {
                Tipo = "Paciente",
                NombreFirmante = nombreFirmante,
                FechaConsentimiento = DateTime.Now
            };

            _context.Consentimientos.Add(consentimiento);
            await _context.SaveChangesAsync();

            paciente.IdConsentimiento = consentimiento.IdConsentimiento;
            await _context.SaveChangesAsync();

            string pdfPath = await _pdfService.GenerarActaPacienteAsync(paciente, consentimiento);
            consentimiento.RutaArchivo = pdfPath;
            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(paciente.Correo, "Acta de Consentimiento Firmada", "Adjunto encontrará su consentimiento firmado.", pdfPath);
            consentimiento.EnviadoPorCorreo = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Paciente");
        }

        // Confirmar y generar acta especialista
        [HttpPost]
        public async Task<IActionResult> ConfirmarConsentimientoEspecialista(int idUsuario, string nombreFirmante)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            if (usuario == null) return NotFound();

            var consentimiento = new Consentimiento
            {
                Tipo = "Especialista",
                NombreFirmante = nombreFirmante,
                FechaConsentimiento = DateTime.Now
            };

            _context.Consentimientos.Add(consentimiento);
            await _context.SaveChangesAsync();

            usuario.IdConsentimiento = consentimiento.IdConsentimiento;
            await _context.SaveChangesAsync();

            string pdfPath = await _pdfService.GenerarActaEspecialistaAsync(usuario, consentimiento);
            consentimiento.RutaArchivo = pdfPath;
            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(usuario.Correo, "Acta de Consentimiento Firmada", "Adjunto encontrará su consentimiento firmado.", pdfPath);
            consentimiento.EnviadoPorCorreo = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Usuario");
        }
    }
}
