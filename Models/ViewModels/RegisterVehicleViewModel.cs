using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
    public class RegisterVehicleViewModel
    {
        [Required]
        public int Id {  get; set; }

        [Required][Remote(action: "CheckUnique", controller: "Vehicles")]
        public string RegistrationNumber {  get; set; }
        public string Brand {  get; set; }
        public string VehicleModel {  get; set; }
        public int VehicleTypeId {  get; set; }
        public string FullName { get; set; }
        public IEnumerable<SelectListItem> VehicleTypes { get; set; }
    }
}
