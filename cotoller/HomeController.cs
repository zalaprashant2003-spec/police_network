using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMsgRepository _sqlmsgrepo;
        private readonly IPoliceRepository _policdrepo;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            IMsgRepository msgRepository,
            IPoliceRepository policdrepo,
            IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _sqlmsgrepo = msgRepository;
            _policdrepo = policdrepo;
            _configuration = configuration;
        }

        // 🔹 Common helper method (to avoid repeating too much code)
        private void SetUserRole()
        {
            string currentUserEmail = User.Identity?.Name;
            ViewBag.CurrentUser = currentUserEmail;

            if (!string.IsNullOrEmpty(currentUserEmail))
            {
                bool isPolice = _policdrepo.isPolice(currentUserEmail);
                ViewBag.UserRole = isPolice ? "Police" : "User";

                // ✅ Read admin email from appsettings.json instead of hardcoding
                string adminEmail = _configuration["AdminSettings:AdminEmail"];
                if (!string.IsNullOrEmpty(adminEmail) && currentUserEmail == adminEmail)
                {
                    ViewBag.UserRole = "Admin";
                }
            }
            else
            {
                ViewBag.UserRole = "Guest";
            }
        }
        [Authorize]
        public IActionResult Index()
        {
            SetUserRole();

            ViewBag.Message = $"Welcome {ViewBag.CurrentUser}";

            // List of all Identity users
            var allUsers = _userManager.Users.ToList();
            ViewBag.AllUsers = allUsers;

            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Send(string Receiver, string Message, double? Latitude, double? Longitude)
        {
            SetUserRole();

            var sender = User.Identity?.Name;

            if (string.IsNullOrEmpty(sender) || string.IsNullOrEmpty(Receiver) || string.IsNullOrEmpty(Message))
            {
                TempData["Error"] = "All fields are required.";
                return RedirectToAction("Index");
            }

            var msg = new Msg
            {
                sender = sender,
                reciever = Receiver,
                message = Message,
                Latitude = Latitude,
                Longitude = Longitude
            };

            _sqlmsgrepo.Add(msg);

            TempData["Success"] = "Message sent successfully!";
            return RedirectToAction("Chat", new { withUser = Receiver });
        }
        [Authorize]
        public IActionResult Chat(string withUser)
        {
            SetUserRole();

            var currentUser = User.Identity?.Name;
            if (string.IsNullOrEmpty(currentUser) || string.IsNullOrEmpty(withUser))
            {
                TempData["Error"] = "Invalid chat selection.";
                return RedirectToAction("Index");
            }

            var messages = _sqlmsgrepo.GetAllMsgs()
                .Where(m => (m.sender == currentUser && m.reciever == withUser) ||
                            (m.sender == withUser && m.reciever == currentUser))
                .OrderBy(m => m.Id)
                .ToList();

            ViewBag.ChatWith = withUser;
            return View(messages);
        }
        public IActionResult Privacy()
        {
            SetUserRole();
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            SetUserRole();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize]
        public IActionResult Inbox()
        {
            SetUserRole();

            var currentUser = User.Identity?.Name;
            if (string.IsNullOrEmpty(currentUser))
            {
                TempData["Error"] = "You must be logged in to view your messages.";
                return RedirectToAction("Index");
            }

            // Get only messages where current user is the receiver
            var receivedMessages = _sqlmsgrepo.GetAllMsgs()
                .Where(m => m.reciever == currentUser)
                .OrderByDescending(m => m.Id) // latest first
                .ToList();

            return View(receivedMessages);
        }

    }
}
