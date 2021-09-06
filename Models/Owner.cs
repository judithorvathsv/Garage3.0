using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
    public class Owner
    {
        [Key]
        [Required(ErrorMessage = "Please enter your social security number!")]
        [Display(Name = "Social Security Number")]
        [RegularExpression("[0-9]{6}-[0-9]{4}", ErrorMessage = "Invalid social security number!")]        
        public string SocialSecurityNumber { get; set; }

        [Required(ErrorMessage = "Please enter firstname!")]
        [Display(Name = "Firstname")]
        [RegularExpression("[-a-zA-Z]+", ErrorMessage = "Invalid firstname!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter lastname!")]
        [Display(Name = "Lastname")]
        [RegularExpression("[-a-zA-Z]+", ErrorMessage = "Invalid lastname!")]
        public string LastName { get; set; }

        List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();


    }
}
