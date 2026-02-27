using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;
using Microsoft.EntityFrameworkCore;


namespace DAL
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly VehicleDbContext _context;

        public PaymentRepository(VehicleDbContext context)
        {
            _context = context;
        }

        public List<Payment> GetAllWithDetails()
        {
            return _context.Payments
                .Include(p => p.Lease)
                    .ThenInclude(l => l.Customer)
                .Include(p => p.Lease)
                    .ThenInclude(l => l.Vehicle)
                .OrderByDescending(p => p.PaymentDate)
                .ToList();
        }

        public void ProcessPayment(Payment payment, Vehicle vehicle)
        {

            _context.Vehicles.Update(vehicle);


            _context.Payments.Add(payment);


            _context.SaveChanges();
        }
    }
}