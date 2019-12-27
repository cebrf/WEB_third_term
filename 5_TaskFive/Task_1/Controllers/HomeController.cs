using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Task_1.Models;
using Task_1.ViewModels;

namespace Task_1.Controllers
{
    public class HomeController : Controller
    {
        List<Patient> patients;
        List<Diagnosis> diagnoses;
        public HomeController()
        {
            diagnoses = new List<Diagnosis>
            {
                new Diagnosis { Id=0, Title="pneumonia", DeathRate=50},
                new Diagnosis { Id=1, Title="tuberculosis", DeathRate=66},
                new Diagnosis { Id=2, Title="blood cancer", DeathRate=85},
                new Diagnosis { Id=3, Title="narcolepsy", DeathRate=47},
                new Diagnosis { Id=4, Title="schizophrenia", DeathRate=19},
                new Diagnosis { Id=5, Title="lupis", DeathRate=61}
            };

            patients = new List<Patient>
            {
                new Patient { Id=0, Name="Mark", DiagnosisId=4},
                new Patient { Id=0, Name="Maria", DiagnosisId=0}
            };
        }
        public IActionResult Index(int? companyId)
        {
            // формируем список компаний для передачи в представление
            List<PatientVM> patientsVM = new List<PatientVM>();
            foreach (var patient in patients)
            {
                Diagnosis diagnosis = diagnoses.Where(di => di.Id == patient.DiagnosisId).FirstOrDefault();
                patientsVM.Add(new PatientVM { Name = patient.Name, Diagnosis = diagnosis.Title });
            }
            return View(patientsVM);
        }
    }
}
