using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
    public class ParkingPlace
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int ParkingPlaceId { get; set; }
        public bool IsOccupied { get; set; }

        // NAV
        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<ParkingEvent> ParkingEvents { get; set; }

    }
}
