using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectFlight.Data;

namespace ProjectFlight.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login (string username, string password)
        {
            throw new NotImplementedException();
        }

        public IActionResult Register(string username, string password, string email, bool isPremium)
        {
            int num;
            User user = new User();
            user.userName = username;
            user.password = password;
            user.email = email;
            user.isPremium = false;
            using (var context = new ApplicationDbContext())
            {
                context.User.Add(user);
                num=context.SaveChanges();
            }
            return new JsonResult(new
            {
                error = num == 0
            });
           
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}