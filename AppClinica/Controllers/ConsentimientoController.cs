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
        private readonly IAesEncryptionService _aes;

        public ConsentimientoController(AppDbContext context, IPdfService pdfService, IEmailService emailService, IAesEncryptionService aes)
        {
            _context = context;
            _pdfService = pdfService;
            _emailService = emailService;
            _aes = aes;
        }

        // Mostrar vista de acta paciente
        [HttpGet]
        public async Task<IActionResult> ActaPaciente(int id)
        {
            var paciente = await _context.Pacientes
                .Include(p => p.Especialista)
                .FirstOrDefaultAsync(p => p.IdPaciente == id);

            if (paciente == null) return NotFound();

            if (paciente.Especialista != null)
            {
                paciente.Especialista.Nombres = _aes.Desencriptar(paciente.Especialista.Nombres);
                paciente.Especialista.Apellidos = _aes.Desencriptar(paciente.Especialista.Apellidos);
            }
            var desencriptado = new Paciente
            {
                Nombres = _aes.Desencriptar(paciente.Nombres),
                Apellidos = _aes.Desencriptar(paciente.Apellidos),
                Telefono = _aes.Desencriptar(paciente.Telefono),
                Direccion = _aes.Desencriptar(paciente.Direccion ?? ""),
                Correo = _aes.Desencriptar(paciente.Correo ?? ""),
                Responsable =  _aes.Desencriptar(paciente.Responsable ?? ""),
                FechaNacimiento = paciente.FechaNacimiento,
                Especialista = paciente.Especialista
            };
            ViewBag.PacienteDesencriptado = desencriptado;
            return View(paciente);
        }

        // Mostrar vista de acta especialista
        [HttpGet]
        public async Task<IActionResult> ActaEspecialista(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Especialista)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null || usuario.Especialista == null) return NotFound();

            var especialista = usuario.Especialista;

            var desencriptado = new Especialista
            {
                Nombres = _aes.Desencriptar(especialista.Nombres),
                Apellidos = _aes.Desencriptar(especialista.Apellidos),
                Especialidad = _aes.Desencriptar(especialista.Especialidad),
                Telefono = _aes.Desencriptar(especialista.Telefono),
                Direccion = _aes.Desencriptar(especialista.Direccion ?? ""),
                JVPP = _aes.Desencriptar(especialista.JVPP)
            };

            ViewBag.EspecialistaDesencriptado = desencriptado;

            return View(especialista); // ✅ Ahora el modelo es del tipo correcto
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
            var PacienteDesencriptado = new Paciente
            {
                Nombres = _aes.Desencriptar(paciente.Nombres),
                Apellidos = _aes.Desencriptar(paciente.Apellidos),
                Telefono = _aes.Desencriptar(paciente.Telefono),
                Direccion = _aes.Desencriptar(paciente.Direccion ?? ""),
                Correo = _aes.Desencriptar(paciente.Correo ?? ""),
                Responsable = _aes.Desencriptar(paciente.Responsable ?? ""),
                FechaNacimiento = paciente.FechaNacimiento,
                Especialista = paciente.Especialista
            };
            _context.Consentimientos.Add(consentimiento);
            await _context.SaveChangesAsync();

            paciente.IdConsentimiento = consentimiento.IdConsentimiento;
            await _context.SaveChangesAsync();

            string pdfPath = await _pdfService.GenerarActaPacienteAsync(PacienteDesencriptado, consentimiento);
            consentimiento.RutaArchivo = pdfPath;
            await _context.SaveChangesAsync();
            var correo = _aes.Desencriptar(paciente.Correo);
            await _emailService.SendEmailAsync(correo, "Acta de Consentimiento Firmada", "Adjunto encontrará su consentimiento firmado.", pdfPath);
            consentimiento.EnviadoPorCorreo = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("verPacientes", "especialista");
        }

        // Confirmar y generar acta especialista
        [HttpPost]
        public async Task<IActionResult> ConfirmarConsentimientoEspecialista(int idUsuario, string nombreFirmante)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Especialista)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);

            if (usuario == null || usuario.Especialista == null) return NotFound();

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

            // Copia desencriptada solo para el PDF
            var desencriptado = new Especialista
            {
                Nombres = _aes.Desencriptar(usuario.Especialista.Nombres),
                Apellidos = _aes.Desencriptar(usuario.Especialista.Apellidos),
                Especialidad = _aes.Desencriptar(usuario.Especialista.Especialidad),
                Telefono = _aes.Desencriptar(usuario.Especialista.Telefono),
                Direccion = _aes.Desencriptar(usuario.Especialista.Direccion ?? ""),
                JVPP = _aes.Desencriptar(usuario.Especialista.JVPP)
            };

            string pdfPath = await _pdfService.GenerarActaEspecialistaAsync(usuario, consentimiento, desencriptado);
            consentimiento.RutaArchivo = pdfPath;
            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(usuario.Correo, "Acta de Consentimiento Firmada", "Adjunto encontrará su consentimiento firmado.", pdfPath);
            consentimiento.EnviadoPorCorreo = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "admin");
        }
    }
}
