using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IVehicleRepository
    {

        List<Vehicle> GetAll();
        Vehicle GetById(int id);
        void Add(Vehicle vehicle);
        void Update(Vehicle vehicle);
        void Delete(int id);


        List<string> GetAvailableBrands();
        List<string> GetModelsByBrand(string brand);
        Vehicle GetVehicleByModel(string model);
    }
}