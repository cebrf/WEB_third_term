using Hospital.Models;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class FilterViewModel
    {
        public FilterViewModel(List<DiagnosisVM> diagnosesVM, int? diagnosisId)
        {
            diagnosesVM.Insert(0, new DiagnosisVM { Id = 0, Title = "All" });
            this.diagnoses = new SelectList(diagnosesVM, "Id", "Title", diagnosisId);
            SelectedDiagnosis = diagnosisId;
        }
        public SelectList diagnoses { get; private set; }
        public int? SelectedDiagnosis { get; private set; }
    }
}
