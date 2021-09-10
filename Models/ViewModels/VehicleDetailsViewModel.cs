using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models.ViewModels
{
    public class VehicleDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Registration number")]      
        public string RegistrationNumber { get; set; }
        
        public string VehicleType { get; set; }

        public string Brand { get; set; }

        [Display(Name = "Vehicle model")]      
        public string VehicleModel { get; set; }

        public int VehicleTypeId { get; set; }

        public string SocialSecurityNumber { get; set; }
        
        [Display(Name = "Firstname")]       
        public string FirstName { get; set; }
   
        [Display(Name = "Lastname")]       
        public string LastName { get; set; }

        [Display(Name = "Name")]
        public string FullName { get; set; }








        //navigation
        public ICollection<ParkingPlace> ParkingPlaces { get; set; }
        public ICollection<ParkingEvent> ParkingEvents { get; set; }
    }
}
