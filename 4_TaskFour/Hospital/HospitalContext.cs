using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Hospital.Models;

namespace Hospital
{
    public class HospitalContext : DbContext
    {
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<Doctors> Doctors { get; set; }
        public HospitalContext(DbContextOptions<HospitalContext> options) :
            base(options)
        {
            Database.EnsureCreated();
        }
    }
}
