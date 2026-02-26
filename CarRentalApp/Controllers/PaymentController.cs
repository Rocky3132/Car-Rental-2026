using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;

namespace CarRentalApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly VehicleDbContext _context;

        public PaymentController(VehicleDbContext context)
        {
            _context = context;
        }

        // GET: Payment/Process
        public IActionResult Process(int vId, int cId, double hrs)
        {
            var vehicle = _context.Vehicles.Find(vId);
            if (vehicle == null) return RedirectToAction("ChooseRide", "Vehicle");

            // 1. Create the Lease Record
            var lease = new Lease
            {
                VehicleID = vId,
                CustomerID = cId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(hrs),
                Type = "Hourly"
            };
            _context.Leases.Add(lease);

            // 2. Mark Vehicle as Rented (This is crucial for your filters)
            vehicle.Status = "Rented";
            _context.Vehicles.Update(vehicle);

            // 3. Create the Payment Record
            var payment = new Payment
            {
                Lease = lease, // EF will automatically link the LeaseID
                PaymentDate = DateTime.Now,
                Amount = (vehicle.DailyRate / 24) * hrs
            };
            _context.Payments.Add(payment);

            // 4. Save everything to the Database in one transaction
            _context.SaveChanges();

            // Pass the payment object to the view to show the receipt
            return View(payment);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            // We join the Lease and Customer tables to get the full story
            var payments = _context.Payments
                .Include(p => p.Lease)
                    .ThenInclude(l => l.Customer)
                .Include(p => p.Lease)
                    .ThenInclude(l => l.Vehicle)
                .OrderByDescending(p => p.PaymentDate)
                .ToList();

            return View(payments);
        }
    }
}