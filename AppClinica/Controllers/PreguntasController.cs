using AppClinica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppClinica.Controllers
{
    public class PreguntasController : Controller
    {
        private readonly AppDbContext _context;

        public PreguntasController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
         public IActionResult ADI_R()
        {
            var preguntas = _context.Preguntas
           .Include(p => p.SeccionTest)
               .ThenInclude(s => s.Test) // necesitas una propiedad de navegación en SeccionTest hacia Tests
           .Where(p => p.SeccionTest.Test.NombreTest == "ADI-R")
           .OrderBy(p => p.Orden)
           .ToList();

            return View(preguntas);
        }
        [HttpGet]
        public IActionResult ADOS2()
        {
            var preguntas = _context.Preguntas
           .Include(p => p.SeccionTest)
               .ThenInclude(s => s.Test) // necesitas una propiedad de navegación en SeccionTest hacia Tests
           .Where(p => p.SeccionTest.Test.NombreTest == "ADOS2")
           .OrderBy(p => p.Orden)
           .ToList();

            return View(preguntas);
        }


    }
}
