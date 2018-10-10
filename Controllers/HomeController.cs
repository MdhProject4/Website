using Microsoft.AspNetCore.Mvc;

namespace ProjectFlight.Controllers
{
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}