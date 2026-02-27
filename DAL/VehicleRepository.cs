using DAL.Exceptions;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace DAL
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleDbContext _context;

        public VehicleRepository(VehicleDbContext context)
        {
            _context = context;
        }

        public List<Vehicle> GetAll() => _context.Vehicles.ToList();


        public Vehicle GetById(int id)
        {
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle == null)
            {

                throw new VehicleNotFoundException($"Vehicle with ID {id} was not found.");
            }
            return vehicle;
        }

        public void Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
        }

        public void Update(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            _context.SaveChanges();
        }


        public void Delete(int id)
        {

            var vehicle = GetById(id);

            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();
        }

        public List<string> GetAvailableBrands() => _context.Vehicles
            .Where(v => v.Status == "Available")
            .Select(v => v.Brand).Distinct().ToList();

        public List<string> GetModelsByBrand(string brand) => _context.Vehicles
            .Where(v => v.Brand == brand && v.Status == "Available")
            .Select(v => v.Model).Distinct().ToList();

        public Vehicle GetVehicleByModel(string model) => _context.Vehicles
            .FirstOrDefault(v => v.Model == model && v.Status == "Available");
    }
}
