using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LeaseRepository : ILeaseRepository
    {
        private readonly VehicleDbContext _context;

        public LeaseRepository(VehicleDbContext context)
        {
            _context = context;
        }


        public List<Lease> GetAllLeasesWithDetails()
        {
            return _context.Leases
                .Include(l => l.Customer)
                .Include(l => l.Vehicle)
                .OrderByDescending(l => l.StartDate)
                .ToList();
        }


        public Lease GetById(int id) => _context.Leases.Find(id);


        public Lease GetLeaseWithVehicle(int id)
        {
            return _context.Leases
                .Include(l => l.Vehicle)
                .Include(l => l.Customer)
               
                .FirstOrDefault(l => l.LeaseID == id);
        }

        public void Add(Lease lease)
        {
            _context.Leases.Add(lease);
            _context.SaveChanges();
        }

        public void Update(Lease lease)
        {
            _context.Leases.Update(lease);
            _context.SaveChanges();
        }
        public List<Lease> GetActiveLeasesByCustomer(string email)
        {
            return _context.Leases
                .Include(l => l.Vehicle) 
                .Include(l => l.Customer) 
                                         
                .Where(l => l.Customer.Email == email && l.EndDate == null)
                .AsNoTracking() 
                .ToList();
        }
    }
}
