using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PoliceController : Controller
    {
        private readonly IPoliceRepository _policeRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        // ✅ Updated constructor to inject IConfiguration
        public PoliceController(IPoliceRepository policeRepository, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _policeRepo = policeRepository;
            _userManager = userManager;
            _configuration = configuration;
        }

        // ✅ Reusable method to set UserRole for all actions
        private void SetUserRole()
        {
            string currentUserEmail = User.Identity?.Name;

            if (!string.IsNullOrEmpty(currentUserEmail))
            {
                bool isPolice = _policeRepo.isPolice(currentUserEmail);
                ViewBag.UserRole = isPolice ? "Police" : "User";

                // ✅ Use admin email from appsettings.json
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

        // GET: /Police
        public IActionResult Index()
        {
            SetUserRole();
            var polices = _policeRepo.GetAllPolices().ToList();
            return View(polices);
        }

        // GET: /Police/Details/5
        public IActionResult Details(int id)
        {
            SetUserRole();
            var police = _policeRepo.GetPoliceById(id);
            if (police == null)
            {
                TempData["Error"] = "Police record not found.";
                return RedirectToAction("Index");
            }
            return View(police);
        }

        // GET: /Police/Create
        public IActionResult Create()
        {
            SetUserRole();
            if (ViewBag.UserRole != "Admin")
                return RedirectToAction("Index", "Home");

            // Pass all Identity users to view
            ViewBag.Users = _userManager.Users.ToList();
            return View();
        }

        // POST: /Police/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string selectedUserId, string PhoneNo, string Place)
        {
            SetUserRole();
            if (ViewBag.UserRole != "Admin")
                return RedirectToAction("Index", "Home");

            if (string.IsNullOrEmpty(selectedUserId) || string.IsNullOrEmpty(PhoneNo) || string.IsNullOrEmpty(Place))
            {
                TempData["Error"] = "All fields are required.";
                return RedirectToAction("Create");
            }

            var user = _userManager.FindByIdAsync(selectedUserId).Result;
            if (user == null)
            {
                TempData["Error"] = "Selected user not found.";
                return RedirectToAction("Create");
            }

            var police = new Police
            {
                Name = user.UserName, // Name comes from Identity
                PhoneNo = PhoneNo,
                Place = Place
            };

            _policeRepo.Add(police);
            TempData["Success"] = "Police record created successfully!";
            return RedirectToAction("Index");
        }

        // GET: /Police/Edit/5
        public IActionResult Edit(int id)
        {
            SetUserRole();
            if (ViewBag.UserRole != "Admin")
                return RedirectToAction("Index", "Home");

            var police = _policeRepo.GetPoliceById(id);
            if (police == null)
            {
                TempData["Error"] = "Police record not found.";
                return RedirectToAction("Index");
            }

            // Pass all Identity users to view
            ViewBag.Users = _userManager.Users.ToList();
            return View(police);
        }

        // POST: /Police/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string selectedUserId, string PhoneNo, string Place)
        {
            SetUserRole();
            if (ViewBag.UserRole != "Admin")
                return RedirectToAction("Index", "Home");

            var police = _policeRepo.GetPoliceById(id);
            if (police == null)
            {
                TempData["Error"] = "Police record not found.";
                return RedirectToAction("Index");
            }

            var user = _userManager.FindByIdAsync(selectedUserId).Result;
            if (user == null)
            {
                TempData["Error"] = "Selected user not found.";
                return RedirectToAction("Edit", new { id });
            }

            police.Name = user.UserName; // Update Name from Identity
            police.PhoneNo = PhoneNo;
            police.Place = Place;

            _policeRepo.Update(police);
            TempData["Success"] = "Police record updated successfully!";
            return RedirectToAction("Index");
        }

        // GET: /Police/Delete/5
        public IActionResult Delete(int id)
        {
            SetUserRole();
            var police = _policeRepo.GetPoliceById(id);
            if (police == null)
            {
                TempData["Error"] = "Police record not found.";
                return RedirectToAction("Index");
            }

            //try
            //{
            //    _policeRepo.Delete(id);
            //    TempData["Success"] = "Police record deleted successfully!";
            //}
            //catch
            //{
            //    TempData["Error"] = "Failed to delete police record.";
            //}

            return View(police);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            SetUserRole();
            var police = _policeRepo.Delete(id);
            if (police != null)
                TempData["Success"] = "Police record deleted successfully!";
            else
                TempData["Error"] = "Failed to delete police record.";
            return RedirectToAction("Index");
        }
    }
}
