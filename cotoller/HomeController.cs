using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        // Constructor injection
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IMsgRepository msgRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _sqlmsgrepo = msgRepository;
        }

        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name; // usually email
                ViewBag.Message = $"Welcome {userName}";
            }
            ViewBag.CurrentUser = User.Identity.Name;
            var allUsers = _userManager.Users.ToList();
            ViewBag.AllUsers = allUsers;
            return View();
        }
        [HttpPost]
        public IActionResult Send(string Receiver, string Message, double? Latitude, double? Longitude)
        {
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

        public IActionResult Chat(string withUser)
        {
            var currentUser = User.Identity?.Name;
            if (string.IsNullOrEmpty(currentUser) || string.IsNullOrEmpty(withUser))
            {
                TempData["Error"] = "Invalid chat selection.";
                return RedirectToAction("Index");
            }

            // Get all messages between currentUser and withUser
            var messages = _sqlmsgrepo.GetAllMsgs()
                .Where(m => (m.sender == currentUser && m.reciever == withUser) ||
                            (m.sender == withUser && m.reciever == currentUser))
                .OrderBy(m => m.Id) // or m.Timestamp if you have
                .ToList();

            ViewBag.ChatWith = withUser;
            return View(messages);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
