using Microsoft.AspNetCore.Mvc;
using ProjectFlight.Data;
using System;

namespace ProjectFlight.Controllers
{
	public class UserController : Controller
    {
        public IActionResult Login (string username, string password)
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
		        Password  = password,
		        Email     = email,
		        IsPremium = false
	        };

			// Temporary variable with number of rows affected
	        int num;

			// Add user to database
	        using (var context = new ApplicationDbContext())
            {
                context.User.Add(user);
                num=context.SaveChanges();
            }

			// Return json result of error
            return new JsonResult(new
            {
                error = num == 0
            });
        }
    }
}