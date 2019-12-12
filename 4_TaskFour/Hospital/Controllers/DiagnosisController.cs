using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hospital.Models;

namespace Hospital.Controllers
{
    public class DiagnosisController : Controller
    {
        HospitalContext db;
        public DiagnosisController(HospitalContext context)
        {
            this.db = context;
        }
        public IActionResult Index()
        {
            return View(db.Diagnoses.ToList());
        }
    }
}