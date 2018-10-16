using Microsoft.AspNetCore.Mvc;
using ProjectFlight.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore.Internal;

namespace ProjectFlight.Controllers
{
	public class UserController : Controller
    {
        public IActionResult Login(string username, string password)
        {
			// TODO
            throw new NotImplementedException();
        }

        public IActionResult Register(string username, string password, string email)
        {
			// Check if any parameter is missing
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
				return new JsonResult(new
				{
					error = true
				});

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
            return new JsonResult(new
            {
                error
            });
        }

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
    }
}