using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;

namespace DAL
{
    public interface ICustomerRepository
    {

        List<Customer> GetAll();


        Customer GetById(int id);
        void Add(Customer customer);
        Customer GetByEmail(string email);
    }
}