using Microsoft.AspNetCore.Mvc;
using Models;
using DAL;
using System.Linq;
using Microsoft.AspNetCore.Authorization; // Add this for security

namespace CarRentalApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly VehicleDbContext _context;

        public CustomerController(VehicleDbContext context)
        {
            _context = context;
        }

        // ================= ADMIN SECTION =================

        // GET: Customer/Index
        // Only Admins can see the list of all customers
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var allCustomers = _context.Customers.ToList();
            return View(allCustomers);
        }

        // ================= CUSTOMER SECTION =================

        // GET: Customer/Create
        public IActionResult Create(int vehicleId, double hours)
        {
            ViewBag.VehicleID = vehicleId;
            ViewBag.Hours = hours;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer, int vehicleId, double hours)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();

                return RedirectToAction("Process", "Payment", new
                {
                    vId = vehicleId,
                    cId = customer.CustomerID,
                    hrs = hours
                });
            }

            ViewBag.VehicleID = vehicleId;
            ViewBag.Hours = hours;
            return View(customer);
        }
    }
}