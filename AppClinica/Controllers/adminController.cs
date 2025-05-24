using AppClinica.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppClinica.Controllers
{
    public class adminController : Controller
    {

        private readonly AppDbContext _context;

        public adminController(AppDbContext context)
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
            return RedirectToAction("Login");
        }



    }
}
