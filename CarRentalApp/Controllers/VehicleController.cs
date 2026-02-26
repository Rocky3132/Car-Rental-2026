using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CarRentalApp.Controllers
{
    public class VehicleController : Controller
    {
        private readonly VehicleDbContext _context;

        public VehicleController(VehicleDbContext context)
        {
            _context = context;
        }

        

        // GET: Vehicle/Index
        [Authorize(Roles ="Admin")]
        public IActionResult Index() => View(_context.Vehicles.ToList());

        // GET: Vehicle/Create
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.Status = "Available"; // New cars start as available
                _context.Vehicles.Add(vehicle);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // GET: Vehicle/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id) => View(_context.Vehicles.Find(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Vehicles.Update(vehicle);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // GET: Vehicle/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id) => View(_context.Vehicles.Find(id));

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // ================= CUSTOMER SECTION =================

        // GET: Vehicle/ChooseRide
        public IActionResult ChooseRide()
        {
            ViewBag.Brands = _context.Vehicles
                .Where(v => v.Status == "Available")
                .Select(v => v.Brand).Distinct().ToList();
            return View();
        }

        public JsonResult GetModels(string brand)
        {
            var models = _context.Vehicles
                .Where(v => v.Brand == brand && v.Status == "Available")
                .Select(v => v.Model).Distinct().ToList();
            return Json(models);
        }

        public JsonResult GetVehicleDetails(string model)
        {
            var vehicle = _context.Vehicles
                .Where(v => v.Model == model && v.Status == "Available")
                .Select(v => new {
                    vehicleID = v.VehicleID,
                    brand = v.Brand,
                    model = v.Model,
                    engineCapacity = v.EngineCapacity,
                    passengerCapacity = v.PassengerCapacity,
                    dailyRate = v.DailyRate
                }).FirstOrDefault();
            return Json(vehicle);
        }
        [HttpGet]
        public JsonResult CalculatePrice(int vehicleId, double hours)
        {
            var vehicle = _context.Vehicles.Find(vehicleId);
            if (vehicle == null) return Json(0);

            // Logic: (DailyRate / 24) * hours
            double total = (vehicle.DailyRate / 24) * hours;

            return Json(total);
        }
    }
}