using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Task_1
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Diagnosis { get; set; }
    }
    public class ApplicationContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=T_6;username=postgres;Password=pswrd");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Patient Patient1 = new Patient { Name = "Tom", Age = 33, Diagnosis= "tuberculosis" };
                Patient Patient2 = new Patient { Name = "Mark", Age = 26, Diagnosis = "pneumonia" };
                Patient Patient3 = new Patient { Name = "Andrew", Age = 26, Diagnosis = "schizophrenia" };
                Patient Patient4 = new Patient { Name = "Stive", Age = 26, Diagnosis = "blood cancer" };

                if (!db.Patients.Any())
                {
                    db.Patients.Add(Patient1);
                    db.Patients.Add(Patient2);
                    db.Patients.Add(Patient3);
                    db.Patients.Add(Patient4);
                    db.SaveChanges();
                }
                var Patients = db.Patients.ToList();
                Console.WriteLine("Patients list:");
                foreach (Patient u in Patients)
                {
                    Console.WriteLine($"{u.Id}.{u.Name} - {u.Age} - {u.Diagnosis}");
                }
            }
        }
    }
}
