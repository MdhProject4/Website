using System.ComponentModel.DataAnnotations;
using ProjectFlight.Enums;

namespace ProjectFlight.Data
{
    public class FlightBookmark
    {
		/// <summary>
		/// ID of the bookmark used by the database
		/// </summary>
		[Key]
		[MaxLength(16)]
		public string Id { get; set; }

        /// <summary>
        /// Username of the user who saved the bookmark
        /// </summary>
        [MaxLength(64)]
        public string Username { get; set; }

        /// <summary>
        /// ID of the flight
        /// </summary>
	    [MaxLength(8)]
        public string SavedId { get; set; }
        
        public EBookmarkType Type { get; set; }
    }

}
