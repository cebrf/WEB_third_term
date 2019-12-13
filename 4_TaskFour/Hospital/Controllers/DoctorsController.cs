using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hospital.Models;
using Microsoft.AspNetCore.Http;

namespace Hospital.Controllers
{
    public class DoctorsController : Controller
    {
        HospitalContext db;
        public DoctorsController(HospitalContext context)
        {
            this.db = context;
        }
        public IActionResult Index(string textToFind = null)
        {
            if (textToFind == null)
            {
                return View(db.Doctors.ToList());
            }           
            else
            {
                List<Doctors> founed = new List<Doctors>();
                foreach (var doc in db.Doctors)
                {
                    if (doc.Id.ToString().Contains(textToFind))
                    {
                        founed.Add(doc);
                    }
                    else if (doc.Name.Contains(textToFind))
                    {
                        founed.Add(doc);
                    }
                    else if (doc.Speciality.Contains(textToFind))
                    {
                        founed.Add(doc);
                    }
                    else if (doc.Skill.ToString().Contains(textToFind))
                    {
                        founed.Add(doc);
                    }
                }
                return View(founed);
            }
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ShowDetails(int id)
        {
            if (id == 0 || db.Doctors.Find(id) == null)
            {
                return NotFound();
            }
            return View(db.Doctors.Find(id));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Add(Doctors doctor)
        {
            if (doctor.Id == 0 || doctor.Name == "" || doctor.Skill == 0 || db.Doctors.Find(doctor.Id) == null)
            {
                return BadRequest();
            }
            db.Doctors.Add(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Edit(int id)
        {
            if (id == 0 || db.Doctors.Find(id) == null)
            {
                return NotFound();
            }
            return View(db.Doctors.Find(id));
        }
        [HttpPost]
        public IActionResult Edit(Doctors doctor)
        {
            if (doctor.Name == "" || doctor.Speciality == "")
            {
                return BadRequest();
            }
            db.Doctors.Update(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            if (id == 0 || db.Doctors.Find(id) == null)
            {
                return NotFound();
            }
            return View(db.Doctors.Find(id));
        }
        [HttpPost]
        public IActionResult Delete(Doctors doctor)
        {
            db.Doctors.Remove(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}