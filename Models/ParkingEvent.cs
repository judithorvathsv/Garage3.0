using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
    public class ParkingEvent
    {
        // FK
        public int ParkingPlaceId { get; set; }
        public int VehicleId { get; set; }


        // NAV
        public ParkingPlace ParkingPlace { get; set; }
        public Vehicle Vehicle { get; set; }


        [Display(Name = "Time of arrival")]
        public DateTime TimeOfArrival { get; set; }

    }
}
