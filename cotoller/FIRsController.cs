using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;
using Microsoft.Extensions.Configuration;

namespace WebApplication1.Controllers
{
    public class FIRsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPoliceRepository _policeRepo;
        private readonly IConfiguration _config;

        public FIRsController(IUnitOfWork unitOfWork, IMapper mapper, IPoliceRepository policeRepo, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _policeRepo = policeRepo;
            _config = config;
        }

        private void SetUserRole()
        {
            string currentUserEmail = User.Identity?.Name;
            ViewBag.CurrentUser = currentUserEmail;

            if (!string.IsNullOrEmpty(currentUserEmail))
            {
                bool isPolice = _policeRepo.isPolice(currentUserEmail);
                ViewBag.UserRole = isPolice ? "Police" : "User";

                var adminEmail = _config["AdminSettings:AdminEmail"];
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
        public ActionResult Index()
        {
            SetUserRole();
            var model = _unitOfWork.FIR.GetAll();
            var vm = _mapper.Map<List<FIRViewModel>>(model);
            return View(vm);
        }

        [Authorize]
        public ActionResult Details(string id)
        {
            SetUserRole();
            var model = _unitOfWork.FIR.GetById(id);
            if (model == null)
                return NotFound();

            var vm = _mapper.Map<FIRViewModel>(model);
            vm.Thieves = model.FIRThieves.Select(ft => ft.Thief.Name).ToList();
            return View(vm);
        }

        [Authorize]
        public ActionResult Create()
        {
            SetUserRole();
            var thievesFromRepo = _unitOfWork.Thief.GetAll();
            var selectList = thievesFromRepo.Select(item => new SelectListItem(item.Name, item.Id)).ToList();

            var vm = new FIRCreateViewModel
            {
                Thieves = selectList,
                SelectedThieves = new List<string>()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(FIRCreateViewModel vm)
        {
            SetUserRole();
            try
            {
                var fir = new FIR
                {
                    Name = vm.Name,
                    Age = vm.Age,
                    Gender = vm.Gender,
                    ContactNo = vm.ContactNo,
                    Address = vm.Address,
                    ComplaintType = vm.ComplaintType,
                    Description = vm.Description,
                    IncidentDate = vm.IncidentDate,
                    FiledDate = DateTime.Now,
                    Status = "Pending",
                    FIRThieves = new List<FIRThief>()
                };

                foreach (var thiefId in vm.SelectedThieves)
                {
                    fir.FIRThieves.Add(new FIRThief { ThiefId = thiefId });
                }

                _unitOfWork.FIR.Insert(fir);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var thievesFromRepo = _unitOfWork.Thief.GetAll();
                vm.Thieves = thievesFromRepo.Select(item => new SelectListItem(item.Name, item.Id)).ToList();
                return View(vm);
            }
        }

        [Authorize]
        public ActionResult Edit(string id)
        {
            SetUserRole();
            var model = _unitOfWork.FIR.GetById(id);
            if (model == null)
                return NotFound();

            var thievesFromRepo = _unitOfWork.Thief.GetAll();
            var selectList = thievesFromRepo.Select(item => new SelectListItem(item.Name, item.Id)).ToList();

            var vm = new FIRCreateViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Age = model.Age,
                Gender = model.Gender,
                ContactNo = model.ContactNo,
                Address = model.Address,
                ComplaintType = model.ComplaintType,
                Description = model.Description,
                IncidentDate = model.IncidentDate,
                Status = model.Status,
                Thieves = selectList,
                SelectedThieves = model.FIRThieves.Select(ft => ft.ThiefId).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(FIRCreateViewModel vm)
        {
            SetUserRole();
            try
            {
                var fir = _unitOfWork.FIR.GetById(vm.Id);
                if (fir == null)
                    return NotFound();

                fir.Name = vm.Name;
                fir.Age = vm.Age;
                fir.Gender = vm.Gender;
                fir.ContactNo = vm.ContactNo;
                fir.Address = vm.Address;
                fir.ComplaintType = vm.ComplaintType;
                fir.Description = vm.Description;
                fir.IncidentDate = vm.IncidentDate;
                fir.Status = vm.Status;

                fir.FIRThieves.Clear();
                foreach (var thiefId in vm.SelectedThieves)
                {
                    fir.FIRThieves.Add(new FIRThief { ThiefId = thiefId });
                }

                _unitOfWork.FIR.Update(fir);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var thievesFromRepo = _unitOfWork.Thief.GetAll();
                vm.Thieves = thievesFromRepo.Select(item => new SelectListItem(item.Name, item.Id)).ToList();
                return View(vm);
            }
        }

        [Authorize]
        public ActionResult Delete(string id)
        {
            SetUserRole();
            var model = _unitOfWork.FIR.GetById(id);
            if (model == null)
                return NotFound();

            var vm = _mapper.Map<FIRViewModel>(model);
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(string id)
        {
            SetUserRole();
            try
            {
                var fir = _unitOfWork.FIR.GetById(id);
                if (fir == null)
                    return NotFound();

                _unitOfWork.FIR.Delete(fir);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Analysis()
        {
            SetUserRole();

            var allFirs = _unitOfWork.FIR.GetAll();

            var analysisData = new FIRAnalysisViewModel
            {
                TotalFIRs = allFirs.Count,
                ByStatus = allFirs.GroupBy(f => f.Status)
                                  .ToDictionary(g => g.Key, g => g.Count()),
                ByComplaintType = allFirs.GroupBy(f => f.ComplaintType)
                                         .ToDictionary(g => g.Key, g => g.Count()),
                ByGender = allFirs.GroupBy(f => f.Gender)
                                  .ToDictionary(g => g.Key, g => g.Count()),
            };

            return View(analysisData);
        }

    }
}
