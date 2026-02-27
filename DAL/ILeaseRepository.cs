using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface ILeaseRepository
    {

        List<Lease> GetAllLeasesWithDetails();
        void Update(Lease lease); 

        Lease GetById(int id);
        Lease GetLeaseWithVehicle(int id);

        void Add(Lease lease);
        List<Lease> GetActiveLeasesByCustomer(string email);
    }

}

