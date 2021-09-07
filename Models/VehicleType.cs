using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
    public class VehicleType
    {
        [Key]    
        public string VehicleTypeId { get; set; }
    
        [Display(Name = "Vehicle type")]
        [Required(ErrorMessage = "Please choose type!")]
        public string Type { get; set; }
     
        public int Size { get; set; }

        List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
