using Microsoft.AspNetCore.Mvc;
using ProjectFlight.Data;

namespace ProjectFlight.Controllers
{
	public class HomeController : Controller
	{
		private ApplicationDbContext context;

	    public HomeController(ApplicationDbContext context) => 
		    this.context = context;

		public IActionResult Index() => 
			View(HttpContext);
	}
}