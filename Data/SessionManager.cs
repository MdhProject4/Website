using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace ProjectFlight.Data
{
	/// <summary>
	/// Session manager used to handle signed in users
	/// </summary>
	public static class SessionManager
	{
		/// <summary>
		/// Add a user to sessions and keep them logged in
		/// </summary>
		/// <param name="context">Current HTTP context</param>
		/// <param name="username">The username of the user to add</param>
		public static async void Add(HttpContext context, string username)
        {
            // If we're already signed in, ignore
	        if (context.User.Claims.Any())
		        return;

	        // Claims
            var claims = new[]
            {
                new Claim("Id", username)
            };

            // Add cookie
            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims)),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMonths(1)
                }
            );
        }

		/// <summary>
		/// Remove a user from sessions and log them out
		/// </summary>
		/// <param name="context">Current HTTP context</param>
		public static async void Remove(HttpContext context) => await context.SignOutAsync();

		/// <summary>
		/// Get the current user's username
		/// </summary>
		/// <param name="context">Current HTTP context</param>
		/// <returns>The username of the current user</returns>
		public static string Get(HttpContext context) =>
			context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

		/// <summary>
		/// Get the current user
		/// </summary>
		/// <param name="context">Current HTTP context</param>
		/// <returns>The current user</returns>
		public static User GetUser(HttpContext context)
		{
			// Get the username of the signed in user
			var username = Get(context);

			// See if we're actually signed in
			if (username == null)
				return null;

			User user;

			// Try to find the user in the database
			using (var db = new ApplicationDbContext())
				user = db.Users.FirstOrDefault(u => u.Username == username);

			return user;
		}
	}
}