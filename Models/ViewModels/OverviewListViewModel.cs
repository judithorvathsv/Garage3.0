using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
    public class OverviewListViewModel
    {
        public IEnumerable<SelectListItem> VehicleTypesSelectList { get; set; }
        public IEnumerable<OverviewViewModel> Overview { get; set; } 

        //SelectListBox
        public string Regnumber { get; set; }
        public VehicleType Types { get; set; }

        public string Type { get; set; }

        // Radiobuttons
        public bool ParkedStatus { get; set; }
        public bool UnparkedStatus { get; set; }
        public bool AllStatus { get; set; }
    }
}
