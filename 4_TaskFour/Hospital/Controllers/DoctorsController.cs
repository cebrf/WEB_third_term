using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hospital.Models;
//using System.Net;

namespace Hospital.Controllers
{
    public class DoctorsController : Controller
    {
        HospitalContext db;
        public DoctorsController(HospitalContext context)
        {
            this.db = context;
        }
        public IActionResult Index()
        {
            return View(db.Doctors.ToList());
        }

        public IActionResult ShowDetails(int id)
        {
            if (db.Doctors.Find(id) == null)
            {
                //NotFound
            }
            return View(db.Doctors.Find(id));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Doctors doctor)
        {
            if (db.Doctors.Find(doctor.Id) == null)
            {
                //BadRequest
            }
            db.Doctors.Add(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (db.Doctors.Find(id) == null)
            {
                //NotFound
            }
            return View(db.Doctors.Find(id));
        }
        [HttpPost]
        public IActionResult Edit(Doctors doctor)
        {
            db.Doctors.Update(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (db.Doctors.Find(id) == null)
            {
                //NotFound
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