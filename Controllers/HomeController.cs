using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProjectFlight.Data;

namespace ProjectFlight.Controllers
{
	public class HomeController : Controller
	{
		/// <summary>
		/// Get all known countries
		/// </summary>
		/// <returns>List with all countries</returns>
		public static IEnumerable<string> GetCountries()
		{
			// List to return later with all values
			var all = new List<string>();

			// Get values from db and add to list
			using (var context = new ApplicationDbContext())
			{
				var departures   = from f in context.FlightInfos where f.Departure   != null select f.DepartureCountry;
				var destinations = from f in context.FlightInfos where f.Destination != null select f.DestinationCountry;

				all.AddRange(departures);
				all.AddRange(destinations);
			}

			// Remove duplicates and sort them
			return all.Distinct().OrderBy(f => f);
		}

		public IActionResult Index() => 
			View(HttpContext);
	}
}