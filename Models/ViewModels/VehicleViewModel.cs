using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
    public class VehicleViewModel
    {
        public int VehicleId { get; set; }
        public string RegistrationNumber { get; set; }
        public string Brand { get; set; }
        public string VehicleModel { get; set; }
        public VehicleType VehicleType { get; set; }
        public bool IsParked { get; set; }
    }
}
