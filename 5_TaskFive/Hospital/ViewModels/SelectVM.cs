using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Hospital.Models;
using Hospital.ViewModels;

namespace Hospital.ViewModels
{
    public class SelectVM
    {
        public IEnumerable<PatientVM> Patients { get; set; }
        public IEnumerable<DiagnosisVM> Diagnoses { get; set; }
    }
}
