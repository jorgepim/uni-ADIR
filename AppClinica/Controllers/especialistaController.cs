using AppClinica.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace AppClinica.Controllers
{

    [Authorize(Roles = "Especialista")]
    public class especialistaController : Controller
    {

        private readonly AppDbContext _context;

        public especialistaController(AppDbContext context)
        {
            _context = context;
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
            if (!ModelState.IsValid)
            {
                return View(paciente);
            }

            // Obtener el ID del especialista actual (desde el claim del usuario logueado)
            var correoUsuario = User.Identity?.Name;
            var especialista = await _context.Especialistas
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(e => e.Usuario.Correo == correoUsuario);

            if (especialista == null)
            {
                TempData["Error"] = "No se pudo determinar el especialista actual.";
                return RedirectToAction("Index");
            }

            paciente.IdEspecialista = especialista.IdEspecialista;
            paciente.FechaRegistro = DateTime.Now;

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


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }



        [HttpGet]
        public IActionResult verPacientes()
        {
            var pacientes = _context.Pacientes
                .Include(p => p.Especialista) // si necesitas incluir datos relacionados
                .ToList();

            return View(pacientes);
        }


    }
}
