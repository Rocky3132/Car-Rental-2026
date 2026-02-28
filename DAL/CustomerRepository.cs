using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;

namespace DAL
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly VehicleDbContext _context;

        public CustomerRepository(VehicleDbContext context)
        {
            _context = context;
        }

        public List<Customer> GetAll() => _context.Customers.ToList();

        public Customer GetById(int id) => _context.Customers.Find(id);

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
        public Customer GetByEmail(string email)
        {
            return _context.Customers.FirstOrDefault(c => c.Email == email);
        }
    }
}