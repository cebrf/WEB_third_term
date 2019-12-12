using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.Models;

namespace Hospital.Models
{
    public class Doctors
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Speciality { get; set; }
        public int Skill { get; set; }
    }
}