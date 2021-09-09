using Garage3.Resources;
using Garage3.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
    public class RegisterMemberViewModel //: IValidatableObject
    {
        [Display(Name = "Social Security Number")]
        [IsRequired][SSNRegCheck]
        public string SocialSecurityNumber { get; set; }

        [Display(Name = "First Name")]
        [IsRequired][NameRegCheck][NameCheck]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [IsRequired][NameRegCheck][NameCheck]
        public string LastName { get; set; }
    }
}
