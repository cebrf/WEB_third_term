using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Patient
    {
        public Patient()
        {
            ArrivalDate = DateTime.Now;
        }
        public int Id { get; set; }
        public int DiagnosisId { get; set; }
        public string Name { get; set; }
        public DateTime ArrivalDate { get; set; }
    }
}
