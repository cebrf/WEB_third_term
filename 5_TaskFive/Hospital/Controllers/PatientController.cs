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
                patientsVM.Add(new PatientVM { Id = patient.Id, Name = patient.Name, Diagnosis = diagnosis.Title, ArrivalDate = patient.ArrivalDate });
            }
            return View(patientsVM);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ShowDetails(int id)
        {
            if (id == 0 || db.Patients.Find(id) == null)
            {
                return NotFound();
            }
            return View(db.Patients.Find(id));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Add(Patient doctor)
        {
            if (doctor.Id == 0 || doctor.Name == "" || db.Patients.Find(doctor.Id) != null)
            {
                return BadRequest();
            }
            db.Patients.Add(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Edit(int id)
        {
            if (id == 0 || db.Patients.Find(id) == null)
            {
                return NotFound();
            }
            return View(db.Patients.Find(id));
        }
        [HttpPost]
        public IActionResult Edit(Patient doctor)
        {
            if (doctor.Name == "")
            {
                return BadRequest();
            }
            db.Patients.Update(doctor);
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
            return View(db.Patients.Find(id));
        }
        [HttpPost]
        public IActionResult Delete(Patient doctor)
        {
            db.Patients.Remove(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}