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
            if (!context.Patients.Any())
            {
                context.Patients.AddRange(
                    new Patient
                    {
                        Name = "Mark",
                        DiagnosisId = 5,
                        ArrivalDate = new System.DateTime(1996, 6, 3, 22, 15, 0)
            },
                    new Patient
                    {
                        Name = "Tom",
                        DiagnosisId = 3,
                        ArrivalDate = new DateTime(1996, 12, 6, 13, 2, 0)
            },
                    new Patient
                    {
                        Name = "Andrew",
                        DiagnosisId = 4,
                        ArrivalDate = new DateTime(1996, 6, 3, 22, 15, 0)
            }
                );
                context.SaveChanges();
            }
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
                        DeathRate = 58
                    },
                    new Diagnosis
                    {
                        Title = "blood cancer",
                        DeathRate = 87
                    },
                    new Diagnosis
                    {
                        Title = "narcolepsy",
                        DeathRate = 27
                    },
                    new Diagnosis
                    {
                        Title = "schizophrenia",
                        DeathRate = 39
                    },
                    new Diagnosis
                    {
                        Title = "lupis",
                        DeathRate = 71
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
