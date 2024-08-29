using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechTest_KawanLama.Data;
using TechTest_KawanLama.Models;

namespace TechTest_KawanLama.Controllers
{
    public class UsersController : Controller
    {
        private readonly DBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(DBContext context,IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Password")] User user)
        {
            string username = _httpContextAccessor.HttpContext.Session.GetString("username");

            if (!string.IsNullOrEmpty(username))
                return RedirectToAction("Index", "Todoes");

            if (ModelState.IsValid && UserExists(user.Username))
            {
                _context.User.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "ToDoes");
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username,Password")] User user)
        {
            var userData = await _context.User.FirstOrDefaultAsync(x => x.Username == user.Username && x.Password == user.Password);
            if (ModelState.IsValid && (userData != null))
            {
                _httpContextAccessor.HttpContext.Session.SetInt32("id", user.Id);
                _httpContextAccessor.HttpContext.Session.SetString("username", user.Username);
            }
            else
            {
                ViewData["Message"] = "Username or password invalid.";
                return View();
            }
            return RedirectToAction("Index", "ToDoes");

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            return RedirectToAction("Login", "Users");

        }

        private bool UserExists(string username)
        {
            return (_context.User?.Any(e => e.Username == username)).GetValueOrDefault();
        }
    }
}
