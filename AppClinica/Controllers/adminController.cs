using Microsoft.AspNetCore.Mvc;

namespace AppClinica.Controllers
{
    public class adminController : Controller
    {
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
            return View();
        }

       

    }
}
