using AppClinica.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace AppClinica.Controllers
{
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

        [HttpPost]
        public IActionResult Addpaciente()
        {
            return View();
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
