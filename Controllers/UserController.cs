using Microsoft.AspNetCore.Mvc;
using ProjectFlight.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ProjectFlight.Controllers
{
	/// <summary>
	/// Controller to handle user related feature
	/// </summary>
	// TODO: Get context instead of creating each time
	public class UserController : Controller
    {
		#region Helpers

	    private static JsonResult GetResult(bool error) => new JsonResult(new { error });

	    private static string HashPassword(string password)
	    {
		    // Create builder for the string
		    var builder = new StringBuilder();

		    // Loop through byte array and convert to (hex) string
		    foreach (var b in Hash(password))
			    builder.Append(b.ToString("x2"));

		    // Return built string
		    return builder.ToString();
	    }

	    private static IEnumerable<byte> Hash(string input)
	    {
		    // Convert input to byte array and hash it
		    using (var sha = SHA256.Create())
			    return sha.ComputeHash(Encoding.UTF8.GetBytes(input));
	    }

		#endregion

		/// <summary>
		/// Tries to login the user with the specified username and password
		/// </summary>
		/// <param name="username">User's username</param>
		/// <param name="password">User's password (not hashed)</param>
		/// <returns>JSON response with error</returns>
		public IActionResult Login(string username, string password)
        {
	        // Check if any parameter is missing
	        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
		        return GetResult(true);

			// Hash entered password
	        var hashedPassword = HashPassword(password);

			// See if it's found in the database
	        bool found;
	        using (var context = new ApplicationDbContext())
		        found = context.Users.Any(u => u.Username == username && u.Password == hashedPassword);

			// TODO: For now, return result without saving login in cookie
	        return GetResult(!found);
        }

		/// <summary>
		/// Tries to create an account
		/// </summary>
		/// <param name="username">User's requested username</param>
		/// <param name="password">User's password</param>
		/// <param name="email">User's optional email</param>
		/// <returns>JSON response with error</returns>
		public IActionResult Register(string username, string password, string email)
        {
			// Check if any parameter is missing
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
				return GetResult(true);

			// User we're adding
			var user = new User
	        {
		        Username  = username,
		        Password  = HashPassword(password),
		        Email     = email,
		        IsPremium = false
	        };

			// Temporary variable to check for errors
	        var error = false;

			// Add user to database
	        using (var context = new ApplicationDbContext())
	        {
		        if (context.Users.Any(u => u.Username == username))
			        error = true;
		        else
		        {
			        context.Users.Add(user);
			        context.SaveChanges();
		        }
            }

			// Return json result of error
            return GetResult(error);
        }

		/// <summary>
		/// Save a flight bookmark
		/// </summary>
		/// <param name="username">Username to save for (will be removed later)</param>
		/// <param name="flightId">Flight ID to save</param>
		/// <returns>JSON response with error</returns>
		// TODO: We don't want to pass username here, check cookie instead
		public IActionResult SaveFlight(string username, string flightId)
        {
           
            using (var context = new ApplicationDbContext())
            {
				// Try to get the user associated with the username
                var user = context.Users.FirstOrDefault(u => u.Username == username);

				// If it wasn't found, return
                if (user == default(User))
                    return GetResult(true);

				// Create the bookmark
	            var bookmark = new FlightBookmark
	            {
		            Username = user.Username,
		            FlightId = flightId,
	            };

				// Try to add it to the database
				context.FlightBookmarks.Add(bookmark);
	            context.SaveChanges();
	            
				// Return error: false
	            return GetResult(false);
            }

        }

        public IActionResult ShowSavedFlights(string username, string flightname)
        {
            using (var context = new ApplicationDbContext())
            {
                var bookmark = context.FlightBookmarks.FirstOrDefault(u => u.Username == username);
                if (bookmark == default(FlightBookmark))
                    return GetResult(true);
                else if (bookmark.Username == username && bookmark.FlightId == flightname)
                {
                    return GetResult(false);
                }
                else
                    return GetResult(true);
            }
        }
    }
}