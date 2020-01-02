using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Task_1
{
    public class Diagnosis
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DeathRate { get; set; }
    }
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int DiagnosisId { get; set; }
        [ForeignKey("DiagnosisId")]
        public virtual Diagnosis Diagnosis { get; set; }
    }
    public class ApplicationContext : DbContext
    {
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseNpgsql("Host=localhost;Port=5432;Database=T_6_4;username=postgres;Password=pswrd");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var patients = db.Patients.ToList();
                foreach (Patient p in patients)
                    Console.WriteLine($"{p.Name} - {p.Diagnosis.Title}");
            }
        }
    }
}