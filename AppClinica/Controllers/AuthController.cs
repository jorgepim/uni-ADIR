
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using AppClinica.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppClinica.Models.ViewModels;
using System.Net.Mail;
using System.Net;
using AppClinica.Services;
using System.Security.Cryptography;

namespace GAlap1p3.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IAesEncryptionService _aes;


        public AuthController(AppDbContext context, IEmailService emailService, IAesEncryptionService aes)
        {
            _context = context;
            _emailService = emailService;
            _aes = aes;
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




                await _context.SaveChangesAsync();

                usuario.IdConsentimiento = null;
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
                return RedirectToAction("Index", "admin");
            else if (usuario.RolId == 2)
                return RedirectToAction("Index", "especialista");

            return RedirectToAction("Login"); // fallback
        }



        // GET: Usuarios/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }





        // RECURERAR CONTRASEÑA
        [HttpGet]
        public ActionResult Recuperar()
        {
            return View();
        }

        // POST: Envío de enlace al correo
        [HttpPost]
        public async Task<IActionResult> Recuperar(RecuperarViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == model.Correo);
            if (usuario == null)
            {
                ModelState.AddModelError("", "Correo no registrado.");
                return View(model);
            }

            // Generar y cifrar token
            var rawToken = Guid.NewGuid().ToString();
            var encryptedToken = _aes.Encriptar(rawToken);

            // Guardar token en base de datos
            usuario.TokenRecuperacion = encryptedToken;
            await _context.SaveChangesAsync();

            // Generar URL con el token sin cifrar
            var callbackUrl = Url.Action("Restablecer", "Auth", new { email = model.Correo, token = rawToken }, protocol: Request.Scheme);
            var body = $"Haz clic aquí para restablecer tu contraseña: <a href='{callbackUrl}'>Restablecer</a>";

            await _emailService.SendEmailAsync(model.Correo, "Restablecer contraseña", body);

            ViewBag.Mensaje = "Se ha enviado un correo para restablecer tu contraseña.";
            return View("RecuperarConfirmacion");
        }

        // GET: Mostrar formulario para nueva contraseña
        [HttpGet]
        public IActionResult Restablecer(string email, string token)
        {
            var model = new RestablecerViewModel { Correo = email, Token = token };
            return View(model);
        }

        // POST: Guardar nueva contraseña
        [HttpPost]
        public async Task<IActionResult> Restablecer(RestablecerViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == model.Correo);
            if (usuario == null)
            {
                ModelState.AddModelError("", "Usuario no encontrado.");
                return View(model);
            }

            // Validar token
            var encryptedInputToken = _aes.Encriptar(model.Token);
            if (string.IsNullOrEmpty(usuario.TokenRecuperacion) || usuario.TokenRecuperacion != encryptedInputToken)
            {
                ModelState.AddModelError("", "Token inválido o expirado.");
                return View(model);
            }

            // ✅ Hashear y guardar nueva contraseña con BCrypt
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(model.NuevaContrasena);

            // Limpiar el token
            usuario.TokenRecuperacion = null;

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Contraseña restablecida con éxito.";
            return RedirectToAction("Login");
        }

    }
}
