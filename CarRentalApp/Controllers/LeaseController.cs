using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace CarRentalApp.Controllers
{
   
    
        
        


        
        public class LeaseController : Controller
        {
            private readonly VehicleDbContext _context;

            public LeaseController(VehicleDbContext context)
            {
                _context = context;
            }

        // GET: Lease/Index
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
            {
                var leases = _context.Leases
                    .Include(l => l.Customer)
                    .Include(l => l.Vehicle)
                    .OrderByDescending(l => l.StartDate)
                    .ToList();

                return View(leases);
            }
        
    

    // GET: Lease/Create?vehicleId=5
    public IActionResult Create(int vehicleId)
        {
            var vehicle = _context.Vehicles.Find(vehicleId);
            if (vehicle == null) return RedirectToAction("ChooseRide", "Vehicle");

            ViewBag.Vehicle = vehicle;
            return View();
        }

        // This action handles the redirection to the Customer controller
        [HttpPost]
        public IActionResult ProceedToCustomer(int vehicleId, double hours)
        {
            // We pass the data to the Customer controller's Create action
            return RedirectToAction("Create", "Customer", new { vehicleId = vehicleId, hours = hours });
        }
    }
}