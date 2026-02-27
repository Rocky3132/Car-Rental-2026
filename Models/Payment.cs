using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [ForeignKey("Lease")]
        public int LeaseID { get; set; }
        public Lease? Lease { get; set; }

        public DateTime? PaymentDate { get; set; }
        public double Amount { get; set; }
    }
}
