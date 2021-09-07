using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
    public class ParkingEvent
    {
        public ParkingPlace ParkingPlace { get; set; }
        public int ParkingPlacesParkingPlaceId { get; set; }

        public Vehicle Vehicle { get; set; }
        public int VehiclesId { get; set; }
    }
}
