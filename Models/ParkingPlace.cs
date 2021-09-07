﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Models
{
    public class ParkingPlace
    {
        [Key]
        public int ParkingPlaceId { get; set; }

        public bool IsOccupied { get; set; }

        List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
