using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ThievesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPoliceRepository _policeRepo;
        private readonly IConfiguration _configuration;

        // ✅ Updated constructor to inject IConfiguration
        public ThievesController(IUnitOfWork unitOfWork, IMapper mapper,
                                 IPoliceRepository policeRepo, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _policeRepo = policeRepo;
            _configuration = configuration;
        }

        // ✅ Reusable helper method to set UserRole
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

        // GET: Thieves
        [Authorize]
        public ActionResult Index()
        {
            SetUserRole();
            var model = _unitOfWork.Thief.GetAll();
            var vm = _mapper.Map<List<ThiefViewModel>>(model);
            return View(vm);
        }

        // GET: Details
        [Authorize]
        public ActionResult Details(string id)
        {
            SetUserRole();
            var model = _unitOfWork.Thief.GetById(id);
            if (model == null)
            {
                TempData["Error"] = "Thief record not found.";
                return RedirectToAction("Index");
            }
            var vm = _mapper.Map<ThiefViewModel>(model);
            return View(vm);
        }

        // GET: Create
        [Authorize]
        public ActionResult Create()
        {
            SetUserRole();
            if (ViewBag.UserRole != "Admin")
            {
                TempData["Error"] = "Only Admin can create thieves.";
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(CreateThiefViewModel vm)
        {
            SetUserRole();
            if (ViewBag.UserRole != "Admin")
            {
                TempData["Error"] = "Only Admin can create thieves.";
                return RedirectToAction("Index");
            }

            try
            {
                var model = _mapper.Map<Thief>(vm);
                _unitOfWork.Thief.Insert(model);
                _unitOfWork.Save();
                TempData["Success"] = "Thief created successfully!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Error"] = "Failed to create thief.";
                return View(vm);
            }
        }

        // GET: Edit
        [Authorize]
        public ActionResult Edit(string id)
        {
            SetUserRole();
            if (ViewBag.UserRole != "Admin")
            {
                TempData["Error"] = "Only Admin can edit thieves.";
                return RedirectToAction("Index");
            }

            var model = _unitOfWork.Thief.GetById(id);
            if (model == null)
            {
                TempData["Error"] = "Thief record not found.";
                return RedirectToAction("Index");
            }

            var vm = _mapper.Map<ThiefViewModel>(model);
            return View(vm);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(ThiefViewModel vm)
        {
            SetUserRole();
            if (ViewBag.UserRole != "Admin")
            {
                TempData["Error"] = "Only Admin can edit thieves.";
                return RedirectToAction("Index");
            }

            try
            {
                var model = _mapper.Map<Thief>(vm);
                _unitOfWork.Thief.Update(model);
                _unitOfWork.Save();
                TempData["Success"] = "Thief updated successfully!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Error"] = "Failed to update thief.";
                return View(vm);
            }
        }

        // GET: Delete
        [Authorize]
        public ActionResult Delete(string id)
        {
            SetUserRole();
            if (ViewBag.UserRole != "Admin")
            {
                TempData["Error"] = "Only Admin can delete thieves.";
                return RedirectToAction("Index");
            }

            var model = _unitOfWork.Thief.GetById(id);
            if (model == null)
            {
                TempData["Error"] = "Thief record not found.";
                return RedirectToAction("Index");
            }

            var vm = _mapper.Map<ThiefViewModel>(model);
            return View(vm);
        }

        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(ThiefViewModel vm)
        {
            SetUserRole();
            if (ViewBag.UserRole != "Admin")
            {
                TempData["Error"] = "Only Admin can delete thieves.";
                return RedirectToAction("Index");
            }

            try
            {
                var model = _mapper.Map<Thief>(vm);
                _unitOfWork.Thief.Delete(model);
                _unitOfWork.Save();
                TempData["Success"] = "Thief deleted successfully!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Error"] = "Failed to delete thief.";
                return View(vm);
            }
        }
    }
}
