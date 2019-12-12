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
                        Title = "d1",
                        DeathRate = 74
                    },
                    new Diagnosis
                    {
                        Title = "d2",
                        DeathRate = 13
                    },
                    new Diagnosis
                    {
                        Title = "d3",
                        DeathRate = 89
                    }
                );
                context.SaveChanges();
            }
            if (!context.Doctors.Any())
            {
                context.Doctors.AddRange(
                    new Doctors
                    {
                        Name = "q1",
                        Speciality = "doctor1",
                        Skill = 12
                    },
                    new Doctors
                    {
                        Name = "q2",
                        Speciality = "doctor2",
                        Skill = 13
                    },
                    new Doctors
                    {
                        Name = "q3",
                        Speciality = "doctor3",
                        Skill = 14
                    },
                    new Doctors
                    {
                        Name = "q3",
                        Speciality = "doctor3",
                        Skill = 14
                    }
                );
                context.SaveChanges();

            }
        }
    }
}
