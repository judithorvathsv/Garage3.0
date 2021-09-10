using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
    public class VehicleTypeModel
    {
        [DisplayName("Type")]
        public int VehicleTypeId { get; set; }

        public IEnumerable<SelectListItem> VehicleTypes { get; set; }
    }
}
