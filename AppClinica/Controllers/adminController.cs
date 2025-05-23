using Microsoft.AspNetCore.Mvc;

namespace AppClinica.Controllers
{
    public class adminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
