using Microsoft.AspNetCore.Mvc;

namespace Studievereniging.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Activities");
        }
    }
}
