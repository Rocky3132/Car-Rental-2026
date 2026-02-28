using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;
namespace DAL
{
    public interface IPaymentRepository
    {

        List<Payment> GetAllWithDetails();


        void ProcessPayment(Payment payment, Vehicle vehicle);
    }
}