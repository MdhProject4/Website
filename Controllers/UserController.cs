using Microsoft.AspNetCore.Mvc;
using ProjectFlight.Data;
using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

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
			// User we're adding
	        var user = new User
	        {
		        Username  = username,
		        Password  = HashPassword(password),
		        Email     = email,
		        IsPremium = false
	        };

			// Temporary variable with number of rows affected
	        int num;

			// Add user to database
	        using (var context = new ApplicationDbContext())
            {
                context.Users.Add(user);
                num = context.SaveChanges();
            }

			// Return json result of error
            return new JsonResult(new
            {
                error = num == 0
            });
        }

	    public string HashPassword(string password)
	    {
		    // Generate a 128-bit salt
		    var salt = new byte[128 / 8];
		    using (var rng = RandomNumberGenerator.Create())
			    rng.GetBytes(salt);

		    // Derive a 256-bit key
		    return Convert.ToBase64String(KeyDerivation.Pbkdf2(
			    password,					// Password
			    salt,						// Salt
			    KeyDerivationPrf.HMACSHA1,	// Prf
			    10000,						// Iteration count
			    256 / 8));					// Num bytes requested
		}
    }
}