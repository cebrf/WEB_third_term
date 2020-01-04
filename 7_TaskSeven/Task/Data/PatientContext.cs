using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task.Models;

namespace Task.Data
{
    public class PatientContext : DbContext
    {
        public PatientContext (DbContextOptions<PatientContext> options)
            : base(options)
        {
        }

        public DbSet<Task.Models.Patient> Patient { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("User ID=postgres;Password=pswrd;Host=localhost;Port=5432;Database=T_7;Pooling=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().HasData(
                new Patient
                {
                    Id = 1,
                    Name = "Mark",
                    Diagnosis = "pneumonia"
                },
                new Patient
                {
                    Id = 2,
                    Name = "Hamilton",
                    Diagnosis = "tuberculosis"
                },
                new Patient
                {
                    Id = 3,
                    Name = "James",
                    Diagnosis = "blood cancer"
                },
                new Patient
                {
                    Id = 4,
                    Name = "Alexceander",
                    Diagnosis = "schizophrenia"
                }
            );
        }
    }
}
