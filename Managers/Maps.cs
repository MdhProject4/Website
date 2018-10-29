using System;
using System.Net;
using Newtonsoft.Json;

namespace ProjectFlight.Managers
{
	/// <summary>
	/// Coordinates for longitude and latitude
	/// </summary>
	public struct Coordinate
	{
		public float Longitude;
		public float Latitude;

		public Coordinate(float longitude, float latitude)
		{
			Longitude = longitude;
			Latitude  = latitude;
		}
	}

	/// <summary>
	/// Contains methods related to maps
	/// </summary>
	public static class Maps
	{
		private static string Get(string url)
		{
			using (var client = new WebClient())
			{
				client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:64.0) Gecko/20100101 Firefox/64.0");
				return client.DownloadString(url);
			}
		}

		/// <summary>
		/// Finds a location (using OpenStreetMap)
		/// </summary>
		/// <param name="query">Location to search for</param>
		/// <returns>Coordinates of the first location found</returns>
		public static Coordinate Find(string query)
		{
			// No error checking? I also like to live dangerously...

			dynamic json = JsonConvert.DeserializeObject(Get($"https://nominatim.openstreetmap.org/search/{query}?format=json"));
			if (json.Count > 0)
			{
				return new Coordinate
				{
					Latitude  = json[0].lat,
					Longitude = json[0].lon
				};
			}
			return new Coordinate();
		}

		private static double DegreesToRadians(double degrees) => degrees * Math.PI / 180;
		
		// TODO: Not sure how accurate this is
		public static double GetDistance(Coordinate c1, Coordinate c2)
		{
			/*
			 * This uses the 'haversine' formula to calculate the distance between two points.
			 * More info: http://www.movable-type.co.uk/scripts/latlong.html
			 * I have no clue how it works tbh, but it does, I guess
			 */

			var distLat = DegreesToRadians(c2.Latitude - c1.Latitude);
			var distLng = DegreesToRadians(c2.Longitude - c1.Longitude);

			var lat1 = DegreesToRadians(c1.Latitude);
			var lat2 = DegreesToRadians(c2.Latitude);

			// This does things, no clue what tbh
			var a = Math.Sin(distLat / 2) * Math.Sin(distLat / 2) + Math.Sin(distLng / 2) * Math.Sin(distLng / 2) * Math.Cos(lat1) * Math.Cos(lat2);
			var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

			// Earth's radius is 6371 meters, unless it's flat
			return 6371e3 * c;
		}
	}
}