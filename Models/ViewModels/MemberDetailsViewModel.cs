using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
    public class MemberDetailsViewModel
    {
        public int Id { get; set; }
        [Key]       
        [Display(Name = "Social Security Number")]      
        public string SocialSecurityNumber { get; set; }
     
        [Display(Name = "Firstname")]       
        public string FirstName { get; set; }

    
        [Display(Name = "Lastname")]        
        public string LastName { get; set; }

        [Display(Name = "Name")]
        public string FullName { get; set; }

        [Display(Name = "Number of vehicles")]
        public int NumberOfVehicles { get; set; }

   

    }
}
