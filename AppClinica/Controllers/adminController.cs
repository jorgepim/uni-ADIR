using AppClinica.Models;

using AppClinica.Models.ViewModels;
using AppClinica.Services;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AppClinica.Controllers
{

    [Authorize(Roles = "Administrador")]
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
            // Validar si el correo ya existe
            bool correoExiste = await _context.Usuarios.AnyAsync(u => u.Correo == model.Usuario.Correo);
            if (correoExiste)
            {
                ModelState.AddModelError("Usuario.Correo", "El correo ya está registrado.");
                model.EspecialidadesDisponibles = new List<string>
    {
        "Psicología Clínica", "Neuropsicología", "Pediatría", "Psiquiatría", "Terapia Ocupacional"
    };
                return View(model);
            }

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
            especialista.JVPP = _aes.Encriptar(especialista.JVPP);
            especialista.Telefono = _aes.Encriptar(especialista.Telefono);
            especialista.Direccion = _aes.Encriptar(especialista.Direccion ?? "");

            _context.Especialistas.Add(especialista);
            await _context.SaveChangesAsync();

            return RedirectToAction("ActaEspecialista", "Consentimiento", new { id = usuario.IdUsuario });
        }
        [HttpGet]
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
        public async Task<IActionResult> Gestionar_usuarios(Usuario id)
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Especialista)
                .ToListAsync();

            var usuariosSinPacientes = new List<Usuario>();

            foreach (var usuario in usuarios)
            {
                if (usuario.RolId == 2) // Especialista
                {
                    var especialista = await _context.Especialistas.FirstOrDefaultAsync(e => e.IdUsuario == usuario.IdUsuario);
                    if (especialista != null)
                    {
                        var tienePacientes = await _context.Pacientes.AnyAsync(p => p.IdEspecialista == especialista.IdEspecialista);
                        if (!tienePacientes)
                        {
                            usuariosSinPacientes.Add(usuario);
                        }
                    }
                }
                else
                {
                    usuariosSinPacientes.Add(usuario); // Admin u otro rol
                }
            }

            return View(usuariosSinPacientes);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (usuario == null)
                return NotFound();

            var especialista = await _context.Especialistas.FirstOrDefaultAsync(e => e.IdUsuario == id);
            if (especialista != null)
            {
                var tienePacientes = await _context.Pacientes.AnyAsync(p => p.IdEspecialista == especialista.IdEspecialista);
                if (tienePacientes)
                {
                    TempData["Error"] = "No se puede editar un usuario que tiene pacientes asignados.";
                    return RedirectToAction("Gestionar_usuarios");
                }
            }

            var model = new UsuarioEditarViewModel
            {
                IdUsuario = usuario.IdUsuario,
                NombreUsuario = usuario.NombreUsuario,
                Correo = usuario.Correo
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(UsuarioEditarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuarioExistente = await _context.Usuarios.FindAsync(model.IdUsuario);
            if (usuarioExistente == null)
                return NotFound();

            var especialista = await _context.Especialistas.FirstOrDefaultAsync(e => e.IdUsuario == model.IdUsuario);
            if (especialista != null)
            {
                var tienePacientes = await _context.Pacientes.AnyAsync(p => p.IdEspecialista == especialista.IdEspecialista);
                if (tienePacientes)
                {
                    TempData["Error"] = "No se puede editar un usuario que tiene pacientes asignados.";
                    return RedirectToAction("Gestionar_usuarios");
                }
            }

            usuarioExistente.NombreUsuario = model.NombreUsuario;
            usuarioExistente.Correo = model.Correo;

            if (!string.IsNullOrWhiteSpace(model.Contrasena))
            {
                usuarioExistente.Contrasena = BCrypt.Net.BCrypt.HashPassword(model.Contrasena);
            }

            _context.Update(usuarioExistente);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Usuario actualizado correctamente.";
            return RedirectToAction("Gestionar_usuarios");
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
