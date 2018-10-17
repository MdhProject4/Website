using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjectFlight.Data
{
	/// <summary>
	/// Updates the flight info database in the background
	/// </summary>
	public class FlightInfoUpdater
	{
        /// <summary>
        /// The limit of entries to add to the database
        /// </summary>
        private readonly int limit;

        /// <summary>
        /// Fetch new flight infos from the API
        /// </summary>
        private FlightInfoResponse[] flightInfoResponses
        {
            get
            {
                // Fetch response from API server
                string response;
                using (var client = new WebClient())
                    response = client.DownloadString("https://public-api.adsbexchange.com/VirtualRadar/AircraftList.json");
                    
                // Parse the response
                dynamic json = JsonConvert.DeserializeObject(response);
                return json.acList.ToObject<FlightInfoResponse[]>();
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="limit">Limit on how many entries to save to the database</param>
        // TODO: Remove constructor and make it an abstract class
        public FlightInfoUpdater(int planeLimit)
        {
            limit = planeLimit;
            Task.Run(() => UpdateFlightInfos());
        }

        /// <summary>
		/// Starts updating flight info on the current thread
		/// </summary>
		private void UpdateFlightInfos()
		{
            // First overwrite all current entries
            Overwrite();

            // Then refresh every minute for now
            while (true)
            {
                Refresh();
                Task.Delay(TimeSpan.FromSeconds(60));
            }
	    }

        /// <summary>
        /// Removes all current entries and refreshes with new ones
        private void Overwrite()
        {
            // TODO: Convert the response to FlightInfo
            var infos = new List<FlightInfo>();
            foreach (var flight in flightInfoResponses)
		    {
                // Check if we've added more than our limit
                if (infos.Count >= limit)
                    continue;

                // Add FlightInfo to list
                var info = new FlightInfo(flight);
                infos.Add(info);
			}

            // Save to database and overwrite current entries
            WriteChanges(infos, true);
        }

        /// <summary>
        /// Updates the current entries
        /// </summary>
        private void Refresh()
        {
            // First get the new flight info
            var newFlights = flightInfoResponses;

            using (var context = new ApplicationDbContext())
            {
                // TODO: Some error checking is probably needed
                foreach(var flight in newFlights)
                {
                    // Find the old result and delete it
                    context.FlightInfos.Remove(context.FlightInfos.FirstOrDefault(f => f.Id == flight.Icao));

                    // Add the new one
                    context.FlightInfos.Add(new FlightInfo(flight));
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Writes changes to the database
        /// </summary>
        /// <param name="infos">The info to save to the database</param>
        /// <param name="emptyTable">Empty the table before adding values</param>
        // TODO: Remove this?
        private void WriteChanges(IEnumerable<FlightInfo> infos, bool emptyTable = false)
        {
            using (var context = new ApplicationDbContext())
            {
                // See if we should empty it first
                if (emptyTable)
                    context.Users.RemoveRange(context.Users);

                // Add infos from list to dbset
                foreach (var info in infos)
                {
                    // Database type time can't store values higher than 24 hours
                    if (info.Tracked.TotalHours >= 24)
                        info.Tracked = TimeSpan.Parse("23:59:59.9999999");

                    context.FlightInfos.Add(info);
                }

				context.SaveChanges();
			}
        }
    }
}