using Microsoft.EntityFrameworkCore;
using ProjectFlight.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectFlight.Managers
{
	/// <summary>
	/// Checks for notifications
	/// </summary>
	public static class NotificationManager
	{
		/// <summary>
		/// Cache for airport locations
		/// </summary>
		private static Dictionary<string, Coordinate> airports;

		/// <summary>
		/// Sets everything up, like a constructor sort of
		/// </summary>
		public static void Create()
		{
			// Create dictionary
			airports = new Dictionary<string, Coordinate>();

			// Check database for old entries
			// (compares flight infos with notifications)
			using (var context = new ApplicationDbContext())
			{
				// Create local copy so we can iterate it while deleting from database
				var notifications = context.FlightNotifications;

				// Check notifications
				foreach (var notification in notifications)
				{
					if (!context.FlightInfos.Any(f => f.Id == notification.FlightId))
						context.FlightNotifications.Remove(notification);
				}

				// Save changes and log results
				var changes = context.SaveChanges();
				if (changes > 0)
					Console.WriteLine($"Removed {changes} invalid {(changes == 1 ? "notification" : "notifications")}");
			}

			// Crate web socket deliverer
			WebSocketDeliverer.Create();
		}

		/// <summary>
		/// Fills airport cache dictionary
		/// </summary>
		// TODO: This cache could be saved in the database
		private static void UpdateAirportCache(IEnumerable<FlightNotification> notifications)
		{
			// Loop through all notifications
			foreach (var notification in notifications)
			{
				// Get full flight info
				var flightInfo = GetFlightInfo(notification.FlightId);

				// Check departure
				if (!airports.ContainsKey(flightInfo.DepartureId))
					airports[flightInfo.DepartureId] = Maps.Find(flightInfo.Departure);

				// Check destination
				if (!airports.ContainsKey(flightInfo.DestinationId))
					airports[flightInfo.DestinationId] = Maps.Find(flightInfo.Destination);
			}
		}

		private static FlightInfo GetFlightInfo(string id)
		{
			using (var context = new ApplicationDbContext())
				return context.FlightInfos.FirstOrDefault(f => f.Id == id);
		}

		/// <summary>
		/// Update and check if any notifications should be sent
		/// </summary>
		/// <param name="infos"></param>
		public static void Update(FlightInfo[] infos)
		{
			using (var context = new ApplicationDbContext())
			{
				// Shorthand
				var notifications = context.FlightNotifications;

				// If it's empty, just return
				if (!notifications.Any())
					return;

				// Update airport cache
				UpdateAirportCache(notifications);

				// Loop through notifications to try and find matches
				foreach (var notification in notifications)
				{
					// If already notified, ignore
					if (notification.Notified)
						continue;

					// Get flight info
					var info = infos.FirstOrDefault(f => f.Id == notification.FlightId);

					// If info not found, continue loop (instead of crashing)
					if (info == null)
						continue;

					// Get time remaining
					var distance = Maps.GetDistance(new Coordinate(info.Longitude, info.Latitude), airports[info.DestinationId]) / 1000f;
					var hours = distance / info.SpeedKm;

					// See if we should remind
					if (hours < 1)
					{
						WebSocketDeliverer.Send(notification.Username, notification.FlightId);
						notification.Notified = true;
					}
				}

				// Save changes
				context.SaveChanges();
			}
		}
	}
}