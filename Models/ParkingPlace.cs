using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
    public class ParkingPlace
    {
        [Key]
        public int Id { get; set; }

        public bool IsOccupied { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }

        public ICollection<ParkingEvent> ParkingEvents { get; set; }

    }
}
