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

        public IActionResult SaveFlight(string username, string flightName)
        {
           
            using (var context = new ApplicationDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == username);
                if (user == default(User))
                    return GetResult(true);
                else if (!user.IsPremium)
                    return GetResult(true);
                else
                {
                    var bookmark = new Bookmarks
                    {
                        Username = user.Username,
                        SavedFlights = flightName,
                    };
                    return GetResult(false);
                }
            }

        }

        public IActionResult ShowSavedFlights(string username, string flightname)
        {
            using (var context = new ApplicationDbContext())
            {
                var bookmark = context.Bookmarks.FirstOrDefault(u => u.Username == username);
                if (bookmark == default(Bookmarks))
                    return GetResult(true);
                else if (bookmark.Username == username && bookmark.SavedFlights == flightname)
                {
                    return GetResult(false);
                }
                else
                    return GetResult(true);
            }
        }
    }
}