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
        public IActionResult Index()
        {
            List<PatientVM> patientsVM = new List<PatientVM>();
            foreach (var patient in db.Patients.ToList())
            {
                var e = db.Diagnoses.ToList();
                Diagnosis diagnosis = db.Diagnoses.ToList().Where(di => di.Id == patient.DiagnosisId).FirstOrDefault();
                patientsVM.Add(new PatientVM(patient.ArrivalDate) { Id = patient.Id, Name = patient.Name, Diagnosis = diagnosis.Title });
            }
            return View(patientsVM);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Add(AddPatientVM pat)
        {
            Diagnosis diagnosis = db.Diagnoses.ToList().Where(di => di.Title == pat.Diagnosis).FirstOrDefault();
            if (pat.Name == null || diagnosis == null)
            {
                return BadRequest();
            }
            db.Patients.Add(new Patient { Name = pat.Name, DiagnosisId = diagnosis.Id });
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