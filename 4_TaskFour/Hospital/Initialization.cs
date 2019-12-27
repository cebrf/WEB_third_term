using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital
{
    public class Initialization
    {
        public static void Init(HospitalContext context)
        {
            if (!context.Diagnoses.Any())
            {
                context.Diagnoses.AddRange(
                    new Diagnosis
                    {
                        Title = "pneumonia",
                        DeathRate = 50
                    },
                    new Diagnosis
                    {
                        Title = "tuberculosis",
                        DeathRate = 66
                    },
                    new Diagnosis
                    {
                        Title = "blood cancer",
                        DeathRate = 84
                    },
                    new Diagnosis
                    {
                        Title = "narcolepsy",
                        DeathRate = 47
                    },
                    new Diagnosis
                    {
                        Title = "schizophrenia",
                        DeathRate = 7
                    },
                    new Diagnosis 
                    {
                        Title = "lupis",
                        DeathRate = 65
                    }

                );
                context.SaveChanges();
            }
            if (!context.Doctors.Any())
            {
                context.Doctors.AddRange(
                    new Doctors
                    {
                        Name = "Tom",
                        Speciality = "Pulmonologist",
                        Skill = 54
                    },
                    new Doctors
                    {
                        Name = "Hamilton",
                        Speciality = "Psychiatrist",
                        Skill = 67
                    },
                    new Doctors
                    {
                        Name = "James",
                        Speciality = "Oncologist",
                        Skill = 70
                    },
                    new Doctors
                    {
                        Name = "Alexceander",
                        Speciality = "Rheumatologist",
                        Skill = 50
                    }
                );
                context.SaveChanges();

            }
        }
    }
}
