using System.ComponentModel.DataAnnotations;

namespace ProjectFlight.Data
{
	public class FlightNotification
	{
		/// <summary>
		/// Id of the notification used by the database
		/// </summary>
		[Key]
		[MaxLength(64)]
		public string Id { get; set; }

		/// <summary>
		/// Username of the user who should be notified
		/// </summary>
		[MaxLength(64)]
		public string Username { get; set; }

		/// <summary>
		/// ID of the flight
		/// </summary>
		[MaxLength(8)]
		public string FlightId { get; set; }

		/// <summary>
		/// If the user has already been notified
		/// </summary>
		public bool Notified { get; set; }
	}
}