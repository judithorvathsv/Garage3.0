using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
    public class RegisterVehicleViewModel
    {
        public string SocialSecurityNumber {  get; set; }
        public string Name {  get; set; }
        public string RegistrationNumber {  get; set; }
        public string Brand {  get; set; }
        public string VehicleModel {  get; set; }
        public VehicleType VehicleType {  get; set; }
        public IEnumerable<SelectListItem> VehicleTypes { get; set; }
    }
}
