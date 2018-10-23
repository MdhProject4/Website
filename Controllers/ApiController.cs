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
	    /// <param name="recent">If the flight has been updated in the past hour</param>
	    /// <param name="limit">Maximum number of results returned</param>
	    /// <param name="query">Only show flights matching the query for departure or destination location</param>
	    /// <returns><see cref="JsonResult"/> of all results</returns>
	    public IActionResult GetFlights(bool depDest = false, bool inAir = false, bool today = false, bool recent = false, int limit = 1000, string query = null)
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
	        if (recent)
		        flights = flights.Where(f => f.LastUpdate < DateTime.UtcNow + TimeSpan.FromHours(1));

	        if (query != null)
	        {
				// Make query all lower case
		        query = query.ToLower();

				// Search for any match
		        flights = flights.Where(f =>
			        (f.Departure?.ToLower().Contains(query) ?? false) || (f.Destination?.ToLower().Contains(query) ?? false));
	        }

	        return new JsonResult(flights.Take(limit));
        }

		/// <summary>
		/// Gets a specific flight
		/// </summary>
		/// <param name="id">ID to get flight of</param>
		/// <returns><see cref="JsonResult"/> of error and info</returns>
	    public IActionResult GetFlight(string id)
		{
			// Temporary variable
			FlightInfo info;

			// Try to find flight
			using (var context = new ApplicationDbContext())
				info = context.FlightInfos.FirstOrDefault(f => f.Id == id || f.RegistrationNumber == id);

			// Check if it was found (not null)
			return new JsonResult(new
			{
				error = info == null,
				info
			});
		}
	}
}