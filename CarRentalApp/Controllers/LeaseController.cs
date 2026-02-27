using Microsoft.AspNetCore.Mvc;
using DAL;
using Models;
using Microsoft.AspNetCore.Authorization;



namespace CarRentalApp.Controllers
{
    public class LeaseController : Controller
    {
        private readonly ILeaseRepository _leaseRepo;
        private readonly IVehicleRepository _vehicleRepo;

        public LeaseController(ILeaseRepository leaseRepo, IVehicleRepository vehicleRepo)
        {
            _leaseRepo = leaseRepo;
            _vehicleRepo = vehicleRepo;
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var leases = _leaseRepo.GetAllLeasesWithDetails();
            return View(leases);
        }


        public IActionResult Create(int vehicleId)
        {
            var vehicle = _vehicleRepo.GetById(vehicleId);
            if (vehicle == null) return RedirectToAction("ChooseRide", "Vehicle");

            ViewBag.Vehicle = vehicle;
            return View();
        }


        [HttpPost]
        public IActionResult ProceedToCustomer(int vehicleId)
        {

            return RedirectToAction("Create", "Customer", new { vehicleId = vehicleId });
        }


        [Authorize]
        public IActionResult MyRentals()
        {

            var userEmail = User.Identity?.Name;

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var myCurrentCars = _leaseRepo.GetActiveLeasesByCustomer(userEmail);

            return View(myCurrentCars);
        }




        public IActionResult ReturnSummary(int id)
        {
            var lease = _leaseRepo.GetLeaseWithVehicle(id);
            if (lease == null || !lease.StartDate.HasValue || lease.Vehicle == null) return NotFound();

            TimeSpan duration = DateTime.Now - lease.StartDate.Value;


            double hoursUsed =duration.TotalHours;


            decimal hourlyRate = (decimal)lease.Vehicle.DailyRate / 24;
            decimal totalBill = hourlyRate * (decimal)hoursUsed;

            ViewBag.FinalAmount = Math.Round(totalBill, 2);
            ViewBag.ActualHours = Math.Round(hoursUsed, 1);
            ViewBag.TimeUsedString = $"{(int)duration.TotalHours}h {duration.Minutes}m";

            return View(lease);
        }
    }
}