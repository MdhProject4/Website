using System;
using Microsoft.AspNetCore.Mvc;
using ProjectFlight.Data;
using System.Collections.Generic;
using System.Linq;

namespace ProjectFlight.Controllers
{
	public class ApiController : Controller
    {
	    /// <summary>
	    /// Get all flights stored in the database
	    /// </summary>
	    /// <param name="depDest">Only show flights who have a known departure and destination</param>
	    /// <param name="inAir">Only show flights who are currently in the air</param>
	    /// <param name="today">Only show flights that was added in the last 24 hours</param>
	    /// <param name="query">Only show flights matching the query for departure or destination location</param>
	    /// <returns></returns>
	    public IActionResult GetFlights(bool depDest = false, bool inAir = false, bool today = false, string query = null)
        {
			// First get all values
	        IEnumerable<FlightInfo> flights;
	        using (var context = new ApplicationDbContext())
		        flights = context.FlightInfos.ToList();

			// Check what values to keep
			// Each if takes 1-2 ms, so it doesn't matter
	        if (depDest)
		        flights = flights.Where(f => f.Departure != null && f.Destination != null);
	        if (inAir)
		        flights = flights.Where(f => f.Grounded == false);
	        if (today)
		        flights = flights.Where(f => f.Tracked != TimeSpan.Parse("23:59:59.9999999"));

	        if (query != null)
	        {
				// Make query all lower case
		        query = query.ToLower();

				// Search for any match
		        flights = flights.Where(f =>
			        (f.Departure?.ToLower().Contains(query) ?? false) || (f.Destination?.ToLower().Contains(query) ?? false));
	        }

	        return new JsonResult(flights);
        }
	}
}