using Microsoft.AspNetCore.Mvc;
using Models;
using DAL;
using System.Linq;
using Microsoft.AspNetCore.Authorization;


namespace CarRentalApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _repo;

        public CustomerController(ICustomerRepository repo)
        {
            _repo = repo;
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var allCustomers = _repo.GetAll();
            return View(allCustomers);
        }


        [Authorize]
        public IActionResult Create(int vehicleId)
        {
            ViewBag.UserEmail = User.Identity?.Name;
            ViewBag.VehicleID = vehicleId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(Customer customer, int vehicleId)
        {
            if (ModelState.IsValid)
            {

                _repo.Add(customer);


                return RedirectToAction("Process", "Payment", new
                {
                    vId = vehicleId,
                    cId = customer.CustomerID
                });
            }


            ViewBag.UserEmail = User.Identity?.Name;
            ViewBag.VehicleID = vehicleId;
            return View(customer);
        }
    }
}