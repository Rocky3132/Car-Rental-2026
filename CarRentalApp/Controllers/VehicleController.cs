using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

using DAL.Exceptions;

namespace CarRentalApp.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository _repo;

        public VehicleController(IVehicleRepository repo) => _repo = repo;

        [Authorize(Roles = "Admin")]
        public IActionResult Index() => View(_repo.GetAll());

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(vehicle);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            try { return View(_repo.GetById(id)); }
            catch (VehicleNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(vehicle);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        public IActionResult ChooseRide()
        {
            ViewBag.Brands = _repo.GetAvailableBrands();
            return View();
        }

        public JsonResult GetModels(string brand) => Json(_repo.GetModelsByBrand(brand));

        public JsonResult GetVehicleDetails(string model)
        {
            var v = _repo.GetVehicleByModel(model);
            if (v == null) return Json(null);
            return Json(new { vehicleID = v.VehicleID, brand = v.Brand, model = v.Model, dailyRate = v.DailyRate });
        }
      
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
              
                var vehicle = _repo.GetById(id);
                return View(vehicle);
            }
            catch (VehicleNotFoundException)
            {
                return NotFound();
            }
        }

    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int VehicleID) 
        {
           
          _repo.Delete(VehicleID);
            return RedirectToAction(nameof(Index));
        }
    }
}