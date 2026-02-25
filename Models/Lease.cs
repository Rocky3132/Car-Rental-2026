using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Lease
    {
        [Key]
        public int LeaseID { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleID { get; set; }
        public Vehicle? Vehicle { get; set; }

        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public Customer? Customer { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? Type { get; set; }

    }
}
