using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
    }
}