using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace ProjectFlight.Data
{
	public abstract class SessionManager
	{
        /// <summary>
        /// Add a user to sessions and keep them logged in
        /// </summary>
        public async void Add(HttpContext context, string userId)
        {
            // If we're already signed in, ignore
            if (context.User.Claims.Count() > 0) {
                return;
            }

            // Claims
            var claims = new[]
            {
                new Claim("Id", userId)
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
    }
}