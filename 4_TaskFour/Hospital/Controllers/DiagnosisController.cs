using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hospital.Models;
using Microsoft.AspNetCore.Http;

namespace Hospital.Controllers
{
    public class DiagnosisController : Controller
    {
        HospitalContext db;
        public DiagnosisController(HospitalContext context)
        {
            this.db = context;
        }
        public IActionResult Index(string textToFind = null)
        {
            if (textToFind == null)
            {
                return View(db.Diagnoses.ToList());
            }
            else
            {
                List<Diagnosis> found = new List<Diagnosis>();
                foreach (var dia in db.Diagnoses)
                {
                    if (dia.Id.ToString().Contains(textToFind))
                    {
                        found.Add(dia);
                    }
                    else if (dia.Title.Contains(textToFind))
                    {
                        found.Add(dia);
                    }
                    else if (dia.DeathRate.ToString().Contains(textToFind))
                    {
                        found.Add(dia);
                    }
                }
                return View(found);
            }
        }
        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ShowDetails(int id)
        {
            if (id == 0 || db.Diagnoses.Find(id) == null)
            {
                return NotFound();
            }
            return View(db.Diagnoses.Find(id));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Add(Diagnosis diagnosis)
        {
            if (diagnosis.Id == 0 || diagnosis.Title == "" || diagnosis.DeathRate == 0 || db.Diagnoses.Find(diagnosis.Id) != null)
            {
                return BadRequest();
            }
            db.Diagnoses.Add(diagnosis);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Edit(int id)
        {
            if (id == 0 || db.Diagnoses.Find(id) == null)
            {
                return NotFound();
            }
            return View(db.Diagnoses.Find(id));
        }
        [HttpPost]
        public IActionResult Edit(Diagnosis diagnosis)
        {
            if (diagnosis.Title == "" || diagnosis.DeathRate == 0)
            {
                return BadRequest();
            }
            db.Diagnoses.Update(diagnosis);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            if (id == 0 || db.Diagnoses.Find(id) == null)
            {
                return NotFound();
            }
            return View(db.Diagnoses.Find(id));
        }
        [HttpPost]
        public IActionResult Delete(Diagnosis diagnosis)
        {
            db.Diagnoses.Remove(diagnosis);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}