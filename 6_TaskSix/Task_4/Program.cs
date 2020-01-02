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
        public Diagnosis Diagnosis { get; set; }
    }
    public class ApplicationContext : DbContext
    {
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public ApplicationContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=T_6_4;username=postgres;Password=pswrd");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Diagnosis diagnosis1 = new Diagnosis { Title = "tuberculosis", DeathRate = 20 };
                Diagnosis diagnosis2 = new Diagnosis { Title = "pneumonia", DeathRate = 17 };
                Diagnosis diagnosis3 = new Diagnosis { Title = "schizophrenia", DeathRate = 24 };
                Diagnosis diagnosis4 = new Diagnosis { Title = "blood cancer", DeathRate = 56 };
                if (!db.Diagnoses.Any())
                {
                    db.Diagnoses.AddRange(diagnosis1, diagnosis2, diagnosis3, diagnosis4);
                    db.SaveChanges();
                }

                Patient Patient1 = new Patient { Name = "Tom", Age = 33, Diagnosis =  diagnosis1 };
                Patient Patient2 = new Patient { Name = "Mark", Age = 26, Diagnosis = diagnosis2 };
                Patient Patient3 = new Patient { Name = "Andrew", Age = 33, Diagnosis = diagnosis3 };
                Patient Patient4 = new Patient { Name = "Stive", Age = 26, Diagnosis = diagnosis3 };
                if (!db.Patients.Any())
                {
                    db.Patients.AddRange(Patient1, Patient2, Patient3, Patient4);
                    db.SaveChanges();
                }


                var Patients = (from patient in db.Patients
                              where patient.DiagnosisId == 1
                              select patient).ToList();
                foreach (Patient u in Patients)
                {
                    Console.WriteLine($"{u.Name} - {u.Age} - {u.Diagnosis.Title}");
                }
                Console.WriteLine("\n\n");

                var Patients_ = db.Patients.Where(p => p.Age == 26);
                foreach (Patient u in Patients_)
                {
                    Console.WriteLine($"{u.Name} - {u.Age} - {u.Diagnosis.Title}");
                }
                Console.WriteLine("\n\n");

                var Diagnoses = db.Diagnoses.Where(p => EF.Functions.Like(p.Title, "%ia%"));
                foreach (Diagnosis u in Diagnoses)
                {
                    Console.WriteLine($"{u.Title} - {u.DeathRate}");
                }
                Console.WriteLine("\n\n");

                Diagnosis d = db.Diagnoses.Find(4);
                Console.WriteLine(d.Title + "\n\n");
            }
        }
    }
}