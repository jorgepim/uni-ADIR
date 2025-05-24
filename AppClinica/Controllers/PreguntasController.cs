using Microsoft.AspNetCore.Mvc;

namespace AppClinica.Controllers
{
    public class PreguntasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
         public IActionResult ADI_R()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ADOS2()
        {
            return View();
        }


    }
}
