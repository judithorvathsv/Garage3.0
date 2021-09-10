using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models.ViewModels
{
    public class OwnerDetailsViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string FullName { get; set; }
        public string SocialSecurityNumber { get; set; }

        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}