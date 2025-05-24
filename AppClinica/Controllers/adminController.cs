using AppClinica.Models;

using AppClinica.Models.ViewModels;
using AppClinica.Services;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace AppClinica.Controllers
{
    public class adminController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IAesEncryptionService _aes;

        public adminController(AppDbContext context, IAesEncryptionService aes)
        {
            _context = context;
            _aes = aes;
        }

        [HttpGet]
        public IActionResult Agregar()
        {
            var model = new EspecialistaViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(EspecialistaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.EspecialidadesDisponibles = new List<string>
        {
            "Psicología Clínica", "Neuropsicología", "Pediatría", "Psiquiatría", "Terapia Ocupacional"
        };
                return View(model);
            }

            var usuario = model.Usuario;
            usuario.RolId = 2;
            usuario.Estado = true;
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);
            usuario.FechaCreacion = DateTime.Now;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var especialista = model.Especialista;
            especialista.IdUsuario = usuario.IdUsuario;

            // Encriptar campos sensibles
            especialista.Nombres = _aes.Encriptar(especialista.Nombres);
            especialista.Apellidos = _aes.Encriptar(especialista.Apellidos);
            especialista.Especialidad = _aes.Encriptar(especialista.Especialidad);
            especialista.Telefono = _aes.Encriptar(especialista.Telefono);
            especialista.Direccion = _aes.Encriptar(especialista.Direccion ?? "");

            _context.Especialistas.Add(especialista);
            await _context.SaveChangesAsync();

            return RedirectToAction("ActaEspecialista", "Consentimiento", new { id = usuario.IdUsuario });
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Gestionar_tests()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Gestionar_usuarios()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Gestionar_resultado()
        {
            var resultados = _context.ResultadosTest
            .Include(r => r.Paciente)
            .Include(r => r.Especialista)
            .Include(r => r.Test)
            .ToList();

            return View(resultados);

        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }




    }
}
