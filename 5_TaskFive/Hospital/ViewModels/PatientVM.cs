using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class PatientVM
    {
        public PatientVM()
        {
            Lifetime = DateTime.Now.Subtract(ArrivalDate);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Diagnosis { get; set; }
        public DateTime ArrivalDate { get; set; }
        public TimeSpan Lifetime { get; }
    }
}
