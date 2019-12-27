using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Hospital.Models;

namespace Hospital.Models
{
    public class Diagnosis
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DeathRate { get; set; }
    }
}
