
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using AppClinica.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GAlap1p3.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Usuarios.AnyAsync(u => u.Correo == usuario.Correo))
                {
                    ModelState.AddModelError("Correo", "El correo ya está registrado.");
                    ViewBag.Roles = new SelectList(_context.Roles.ToList(), "IdRol", "NombreRol", usuario.RolId);
                    return View(usuario);
                }

                // Crear consentimiento automático
                var consentimiento = new Consentimiento
                {
                    Tipo = usuario.RolId == 3 ? "Especialista" : "General",
                    NombreFirmante = usuario.NombreUsuario,
                    FechaConsentimiento = DateTime.Now,
                    RutaArchivo = null,
                    EnviadoPorCorreo = false
                };

                _context.Consentimientos.Add(consentimiento);
                await _context.SaveChangesAsync();

                usuario.IdConsentimiento = consentimiento.IdConsentimiento;
                usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);
                usuario.FechaCreacion = DateTime.Now;
                usuario.Estado = true;

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }

            ViewBag.Roles = new SelectList(_context.Roles.ToList(), "IdRol", "NombreRol", usuario.RolId);
            return View(usuario);
        }


        // GET: Usuarios/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Usuarios/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string correo, string clave)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == correo);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(clave, usuario.Contrasena))
            {
                ModelState.AddModelError(string.Empty, "Correo o clave incorrectos.");
                return View();
            }

            if (usuario.Rol == null)
            {
                ModelState.AddModelError(string.Empty, "El usuario no tiene un rol asignado.");
                return View();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.NombreUsuario),
        new Claim(ClaimTypes.Email, usuario.Correo),
        new Claim(ClaimTypes.Role, usuario.Rol.NombreRol)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            if (usuario.RolId == 1)
                return RedirectToAction("Index", "Admin");
            else if (usuario.RolId == 3)
                return RedirectToAction("Index", "Especialista");

            return RedirectToAction("Login"); // fallback
        }



        // GET: Usuarios/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


    }
}
