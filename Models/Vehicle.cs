using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleID { get; set; }

        [Required]
        public string? Brand { get; set; }

        public string? Model { get; set; }

        public int Year { get; set; }

        public double DailyRate { get; set; }

        public string? Status { get; set; }

        public int PassengerCapacity { get; set; }

        public double EngineCapacity { get; set; }


    }
}
