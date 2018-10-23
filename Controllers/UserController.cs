using Microsoft.AspNetCore.Mvc;
using ProjectFlight.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace ProjectFlight.Controllers
{
	/// <summary>
	/// Controller to handle user related feature
	/// </summary>
	public class UserController : Controller
    {
	    private readonly ApplicationDbContext dbContext;

	    public UserController(ApplicationDbContext context) => dbContext = context;

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
	        var found = dbContext.Users.Any(u => u.Username == username && u.Password == hashedPassword);

			// If found, sign in
			if (found)
				SessionManager.Add(HttpContext, username);

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
			    Username = username,
			    Password = HashPassword(password),
			    Email = email,
			    IsPremium = false
		    };

		    // Temporary variable to check for errors
		    var error = false;

		    // Add user to database
		    if (dbContext.Users.Any(u => u.Username == username))
			    error = true;
		    else
		    {
			    dbContext.Users.Add(user);
			    dbContext.SaveChanges();
				// TODO: Login here as well?
		    }

		    // Return json result of error
		    return GetResult(error);
	    }

	    /// <summary>
		/// Save a flight bookmark to the current user
		/// </summary>
		/// <param name="flightId">Flight ID to save</param>
		/// <returns>JSON response with error</returns>
		[Authorize]
		public IActionResult SaveFlight(string flightId)
	    {
		    // Try to get the user associated with the username
		    var user = dbContext.Users.FirstOrDefault(u => u.Username == SessionManager.Get(HttpContext));

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
		    dbContext.FlightBookmarks.Add(bookmark);
		    dbContext.SaveChanges();

		    // Return error: false
		    return GetResult(false);
	    }

	    /// <summary>
		/// Get saved flight bookmarks for the current user
		/// </summary>
		/// <returns>JSON with array of bookmarks</returns>
		[Authorize]
		public IActionResult GetSavedFlights() => 
		    new JsonResult(dbContext.FlightBookmarks.Where(b => b.Username == SessionManager.Get(HttpContext)));

		/// <summary>
		/// Adds a notification about a flight to a user
		/// </summary>
		/// <param name="id">ID of the flight</param>
		/// <returns>JSON response with error</returns>
		[Authorize]
	    public IActionResult AddNotification(string id)
	    {
			// TODO: Check so flightID exists

			// Add it to the database
		    dbContext.FlightNotifications.Add(new FlightNotification
		    {
			    FlightId = id,
			    Username = SessionManager.Get(HttpContext),
			    Notified = false
		    });

			// Try to save
		    var error = false;
		    try
		    {
			    dbContext.SaveChanges();
		    }
		    catch (SqlException)
		    {
			    error = true;
		    }

			// Return if it was successful
		    return GetResult(error);
	    }

		/// <summary>
		/// Get saved notifications for the current user
		/// </summary>
		/// <returns>JSON array of notifications</returns>
		[Authorize]
	    public IActionResult GetNotifications() => 
			new JsonResult(dbContext.FlightNotifications.Where(b => b.Username  == SessionManager.Get(HttpContext)));
    }
}