using System.ComponentModel.DataAnnotations;

namespace ProjectFlight.Data
{
    public class Bookmarks
    {
        /// <summary>
        /// Set a username as primary 
        /// </summary>
        [Key]
        [MaxLength(64)]
        public string Username { get; set; }
        /// <summary>
        /// Creating a row of saved flights 
        /// </summary>
        public string SavedFlights { get; set; }
    }

}
