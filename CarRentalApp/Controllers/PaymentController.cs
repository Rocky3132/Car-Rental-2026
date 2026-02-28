
using Microsoft.AspNetCore.Mvc;
using DAL;
using Models;
using Microsoft.AspNetCore.Authorization;
using System;

namespace CarRentalApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IVehicleRepository _vehicleRepo;
        private readonly ILeaseRepository _leaseRepo;

        public PaymentController(IPaymentRepository paymentRepo, IVehicleRepository vehicleRepo, ILeaseRepository leaseRepo)
        {
            _paymentRepo = paymentRepo;
            _vehicleRepo = vehicleRepo;
            _leaseRepo = leaseRepo;
        }

   
        public IActionResult Process(int vId, int cId)
        {
            var vehicle = _vehicleRepo.GetById(vId);
            if (vehicle == null) return RedirectToAction("ChooseRide", "Vehicle");


            var lease = new Lease
            {
                VehicleID = vId,
                CustomerID = cId,
                StartDate = DateTime.Now,
                EndDate = null, 
                Type = "Hourly"
            };

            
            vehicle.Status = "Rented";

            _leaseRepo.Add(lease);
            _vehicleRepo.Update(vehicle);

            return RedirectToAction("BookingConfirmed");
        }
        public IActionResult BookingConfirmed()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FinalizeReturn(int leaseId, decimal amount)
        {
         
            var lease = _leaseRepo.GetLeaseWithVehicle(leaseId);
            if (lease == null) return NotFound();

            var vehicle = _vehicleRepo.GetById(lease.VehicleID);
            if (vehicle == null) return NotFound();

          
            var payment = new Payment
            {
                LeaseID = leaseId,
                PaymentDate = DateTime.Now,
        
                Amount = (double)amount
            };

          
            lease.EndDate = DateTime.Now;

            vehicle.Status = "Available";

         
            _paymentRepo.ProcessPayment(payment, vehicle);
            _leaseRepo.Update(lease);

         
            return View("Success", payment);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var payments = _paymentRepo.GetAllWithDetails();
            return View(payments);
        }
    }
}