using AppClinica.Models;
using AppClinica.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;



namespace AppClinica.Controllers
{

    [Authorize(Roles = "Especialista")]
    public class especialistaController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IAesEncryptionService _aes;

        public especialistaController(AppDbContext context, IAesEncryptionService aes)
        {
            _context = context;
            _aes = aes;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Paciente paciente)
        {
            var correoUsuario = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            // Buscar al especialista correspondiente
            var especialista = await _context.Especialistas
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(e => e.Usuario.Correo == correoUsuario);

            if (especialista == null)
            {
                TempData["Error"] = "No se pudo determinar el especialista actual.";
                return RedirectToAction("Index");
            }

            // Asignar antes de la validación
            paciente.IdEspecialista = especialista.IdEspecialista;
            paciente.FechaRegistro = DateTime.Now;

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    var key = error.Key;
                    var errors = error.Value.Errors;
                    foreach (var e in errors)
                    {
                        Console.WriteLine($"Error en {key}: {e.ErrorMessage}");
                    }
                }

                return View(paciente);
            }

            paciente.Nombres = _aes.Encriptar(paciente.Nombres);
            paciente.Apellidos = _aes.Encriptar(paciente.Apellidos);
            paciente.Telefono = _aes.Encriptar(paciente.Telefono);
            paciente.Responsable = _aes.Encriptar(paciente.Responsable ?? string.Empty);
            paciente.ParentescoResponsable = _aes.Encriptar(paciente.ParentescoResponsable ?? string.Empty);
            paciente.Direccion = _aes.Encriptar(paciente.Direccion ?? string.Empty);
            paciente.Correo = _aes.Encriptar(paciente.Correo);
           


            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Paciente agregado correctamente.";
            return RedirectToAction("verPacientes");
        }




        [HttpGet]
        public IActionResult Evaluar()
        {
            var pacientes = _context.Pacientes
                .Include(p => p.Especialista)
                .ToList();

            return View(pacientes); // 👈 Esto llena el @model con la lista
        }


        [HttpGet]
        public IActionResult test_adir()
        {
            return View(); // 👈 Esto llena el @model con la lista
        }

        [HttpGet]
        public IActionResult test_ados()
        {
            return View(); // 👈 Esto llena el @model con la lista
        }

        [HttpGet]
        public IActionResult TestAdos(int idPaciente)
        {
            return View(idPaciente); // 👈 Esto llena el @model con la lista
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }



        [HttpGet]
        public IActionResult verPacientes()
        {
            var pacientes = _context.Pacientes
                .Include(p => p.Especialista)
                .ToList();

            foreach (var p in pacientes)
            {
                try
                {
                    p.Nombres = _aes.Desencriptar(p.Nombres);
                    p.Apellidos = _aes.Desencriptar(p.Apellidos);
                    p.Correo = _aes.Desencriptar(p.Correo);
                    p.Telefono = _aes.Desencriptar(p.Telefono);

                    if (!string.IsNullOrWhiteSpace(p.Responsable))
                        p.Responsable = _aes.Desencriptar(p.Responsable);

                    if (!string.IsNullOrWhiteSpace(p.ParentescoResponsable))
                        p.ParentescoResponsable = _aes.Desencriptar(p.ParentescoResponsable);

                    if (!string.IsNullOrWhiteSpace(p.Direccion))
                        p.Direccion = _aes.Desencriptar(p.Direccion);

                    if (p.Especialista != null)
                    {
                        p.Especialista.Nombres = _aes.Desencriptar(p.Especialista.Nombres);
                        p.Especialista.Apellidos = _aes.Desencriptar(p.Especialista.Apellidos);
                    }
                }
                catch (FormatException ex)
                {
                    // Opcional: loguear o manejar la excepción
                    Console.WriteLine($"Error desencriptando paciente ID {p.IdPaciente}: {ex.Message}");
                }
            }

            return View(pacientes);
        }

        public async Task<IActionResult> verInformacion(int id)
        {
            var paciente = await _context.Pacientes
                .Include(p => p.ResultadosTest)
                    .ThenInclude(r => r.Test)
                .Include(p => p.Especialista)
                .FirstOrDefaultAsync(p => p.IdPaciente == id);

            if (paciente == null) return NotFound();

            // Desencriptar campos sensibles
            paciente.Nombres = _aes.Desencriptar(paciente.Nombres);
            paciente.Apellidos = _aes.Desencriptar(paciente.Apellidos);
            paciente.Correo = _aes.Desencriptar(paciente.Correo);
            paciente.Telefono = _aes.Desencriptar(paciente.Telefono);
            paciente.Direccion = _aes.Desencriptar(paciente.Direccion ?? "");
            paciente.Responsable = _aes.Desencriptar(paciente.Responsable ?? "");
            paciente.ParentescoResponsable = _aes.Desencriptar(paciente.ParentescoResponsable ?? "");

            if (paciente.Especialista != null)
            {
                paciente.Especialista.Nombres = _aes.Desencriptar(paciente.Especialista.Nombres);
                paciente.Especialista.Apellidos = _aes.Desencriptar(paciente.Especialista.Apellidos);
            }

            return View(paciente);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var paciente = await _context.Pacientes
                .Include(p => p.Especialista)
                .FirstOrDefaultAsync(p => p.IdPaciente == id);

            if (paciente == null)
                return NotFound();

            // Desencriptar campos
            paciente.Nombres = _aes.Desencriptar(paciente.Nombres);
            paciente.Apellidos = _aes.Desencriptar(paciente.Apellidos);
            paciente.Correo = _aes.Desencriptar(paciente.Correo);
            paciente.Telefono = _aes.Desencriptar(paciente.Telefono);
            paciente.Responsable = string.IsNullOrWhiteSpace(paciente.Responsable) ? null : _aes.Desencriptar(paciente.Responsable);
            paciente.ParentescoResponsable = string.IsNullOrWhiteSpace(paciente.ParentescoResponsable) ? null : _aes.Desencriptar(paciente.ParentescoResponsable);
            paciente.Direccion = _aes.Desencriptar(paciente.Direccion ?? string.Empty);

            return View(paciente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Paciente paciente)
        {
            if (!ModelState.IsValid)
                return View(paciente);

            var pacienteExistente = await _context.Pacientes.FindAsync(paciente.IdPaciente);
            if (pacienteExistente == null)
                return NotFound();

            // Encriptar y actualizar campos
            pacienteExistente.Nombres = _aes.Encriptar(paciente.Nombres);
            pacienteExistente.Apellidos = _aes.Encriptar(paciente.Apellidos);
            pacienteExistente.Correo = _aes.Encriptar(paciente.Correo);
            pacienteExistente.Telefono = _aes.Encriptar(paciente.Telefono);
            pacienteExistente.Responsable = _aes.Encriptar(paciente.Responsable ?? string.Empty);
            pacienteExistente.ParentescoResponsable = _aes.Encriptar(paciente.ParentescoResponsable ?? string.Empty);
            pacienteExistente.Direccion = _aes.Encriptar(paciente.Direccion ?? string.Empty);
            pacienteExistente.FechaNacimiento = paciente.FechaNacimiento;
            pacienteExistente.Sexo = paciente.Sexo;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Datos del paciente actualizados correctamente.";
            return RedirectToAction("verPacientes");
        }

        //public IActionResult IniciarModuloAdos(int modulo, int idPaciente)
        //{
        //    // lógica para redirigir al módulo correspondiente
        //}

    }
}
