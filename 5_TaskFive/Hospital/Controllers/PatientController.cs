using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital.Controllers
{
    public class PatientController : Controller
    {
        HospitalContext db;
        public PatientController(HospitalContext context)
        {
            this.db = context;
        }
        public IActionResult Index(int? DiagnosisId, int Page = 1, SortState SortOrder = SortState.NameAsc)
        {
            int pageSize = 3;

            List<DiagnosisVM> diagnosesVM = db.Diagnoses
                .Select(d => new DiagnosisVM { Id = d.Id, Title = d.Title })
                .ToList();

            List<PatientVM> patientsVM = new List<PatientVM>();
            foreach (var patient in db.Patients.ToList())
            {
                var e = db.Diagnoses.ToList();
                Diagnosis diagnosis = db.Diagnoses.ToList().Where(di => di.Id == patient.DiagnosisId).FirstOrDefault();
                if (DiagnosisId == null || DiagnosisId == 0 || diagnosis.Id == DiagnosisId)
                    patientsVM.Add(new PatientVM(patient.ArrivalDate) { Id = patient.Id, Name = patient.Name, Diagnosis = diagnosis.Title });
            }


            switch (SortOrder)
            {
                case SortState.NameDesc:
                    patientsVM = patientsVM.OrderByDescending(s => s.Name).ToList();
                    break;
                case SortState.DiagnAsc:
                    patientsVM = patientsVM.OrderBy(s => s.Diagnosis).ToList();
                    break;
                case SortState.DiagnDesc:
                    patientsVM = patientsVM.OrderByDescending(s => s.Diagnosis).ToList();
                    break;
                case SortState.DateAsc:
                    patientsVM = patientsVM.OrderBy(s => s.Lifetime).ToList();
                    break;
                case SortState.DateDesc:
                    patientsVM = patientsVM.OrderByDescending(s => s.Lifetime).ToList();
                    break;
                default:
                    patientsVM = patientsVM.OrderBy(s => s.Name).ToList();
                    break;
            }

            var count = patientsVM.Count();
            var items = patientsVM.Skip((Page - 1) * pageSize).Take(pageSize).ToList();

            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, Page, pageSize),
                SortViewModel = new SortViewModel(SortOrder),
                FilterViewModel = new FilterViewModel(diagnosesVM, DiagnosisId),
                Patients = items
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Diagnoses = new SelectList(db.Diagnoses.ToList(), "Id", "Title");
            return View();
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