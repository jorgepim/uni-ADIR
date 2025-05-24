using Microsoft.AspNetCore.Mvc;

namespace AppClinica.Controllers
{
    public class EvaluacionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult evaluar()
        {
            return View();
        }
        public IActionResult Ados2()
        {
            return View();
        }
        public IActionResult Adir()
        {
            return View();
        }
    }
}
