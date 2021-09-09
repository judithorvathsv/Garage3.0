using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models.ViewModels
{
    public class OwnerDetailsViewModel
    {
        [Display(Name = "Social Security Number")]
        public string SocialSecurityNumber { get; set; }

        [Display(Name = "Firstname")]
        public string FirstName { get; set; }


        [Display(Name = "Lastname")]
        public string LastName { get; set; }

        [Display(Name = "Name")]
        public string FullName { get; set; }

        [Display(Name = "RegistrationNumber")]
        public string RegistrationNumber { get; set; }

        public string Brand { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleType { get; set; }
    }
}
