using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Hospital.Controllers
{
    public class PatientController : Controller
    {
        HospitalContext db;
        public PatientController(HospitalContext context)
        {
            this.db = context;
        }
        public IActionResult Index(int DiagnosisId = 0)
        {
            List<DiagnosisVM> diagnosesVM = db.Diagnoses
                .Select(d => new DiagnosisVM { Id = d.Id, Title = d.Title })
                .ToList();
            diagnosesVM.Insert(0, new DiagnosisVM { Id = 0, Title = "All" });

            List<PatientVM> patientsVM = new List<PatientVM>();
            foreach (var patient in db.Patients.ToList())
            {
                var e = db.Diagnoses.ToList();
                Diagnosis diagnosis = db.Diagnoses.ToList().Where(di => di.Id == patient.DiagnosisId).FirstOrDefault();
                if (DiagnosisId == 0 || diagnosis.Id == DiagnosisId)
                    patientsVM.Add(new PatientVM(patient.ArrivalDate) { Id = patient.Id, Name = patient.Name, Diagnosis = diagnosis.Title });
            }

            SelectVM selectVM = new SelectVM { Diagnoses = diagnosesVM, Patients = patientsVM };
            return View(selectVM);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(db.Diagnoses.ToList());
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Add(AddPatientVM pat)
        {
            if (pat.Name == null)
            {
                return BadRequest();
            }
            db.Patients.Add(new Patient { Name = pat.Name, DiagnosisId = pat.DiagnosisId });
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            if (id == 0 || db.Patients.Find(id) == null)
            {
                return NotFound();
            }
            Patient p = db.Patients.Find(id);
            Diagnosis d = db.Diagnoses.ToList().Where(di => di.Id == p.DiagnosisId).FirstOrDefault();
            return View(new PatientVM(p.ArrivalDate) { Id = p.Id, Name = p.Name, Diagnosis = d.Title });
        }
        [HttpPost]
        public IActionResult Delete(PatientVM p)
        {
            db.Patients.Remove(db.Patients.Find(p.Id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}