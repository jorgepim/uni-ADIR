using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppClinica.Controllers
{

    [Authorize(Roles = "Especialista")]
    public class especialistaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
