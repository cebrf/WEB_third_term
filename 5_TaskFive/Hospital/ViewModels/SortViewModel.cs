using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.ViewModels
{
    public class SortViewModel
    {
        public SortState NameSort { get; private set; }
        public SortState DiagnSort { get; private set; }
        public SortState DateSort { get; private set; }
        public SortState Current { get; private set; }

        public SortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            DiagnSort = sortOrder == SortState.DiagnAsc ? SortState.DiagnDesc : SortState.DiagnAsc;
            DateSort = sortOrder == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;
            Current = sortOrder;
        }
    }
}
