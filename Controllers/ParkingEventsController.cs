using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage3.Data;
using Garage3.Models;
using Garage3.Models.ViewModels;

namespace Garage3.Controllers
{
    public class ParkingEventsController : Controller
    {
        private readonly Garage3Context db;
        private const int GarageCapacity = 20;

        public Garage3Context Context => db;

        public ParkingEventsController(Garage3Context context)
        {
            db = context;
        }

        public async Task<IActionResult> Park(int id, int vehicleid)
        {
            var parkingVehicle = await db.Vehicle.Include(v => v.VehicleType).Where(v => v.VehicleId == vehicleid).FirstOrDefaultAsync();
            var parkingPlace = await db.ParkingPlace.AsNoTracking().ToListAsync();

            var numberOfPlaces = parkingPlace.Count();
            //Hämtar det sista inlagda värdet
            var parkingPlaceId = parkingPlace.OrderBy(pp => pp.ParkingPlaceId).Select(pp => pp.ParkingPlaceId).LastOrDefault();
            var firstAvailableParkingSpace = parkingPlace.Where(pp => pp.IsOccupied == false).FirstOrDefault();
            var parkingplace = new ParkingPlace();


            if (parkingPlaceId <= GarageCapacity)
            {
                if (firstAvailableParkingSpace != null)
                {
                    parkingplace.ParkingPlaceId = firstAvailableParkingSpace.ParkingPlaceId;
                    parkingplace.IsOccupied = true;

                    var parkingevent = new ParkingEvent
                    {
                        ParkingPlace = parkingplace,
                        Vehicle = parkingVehicle,
                        TimeOfArrival = DateTime.Now
                    };

                    db.ParkingEvent.Update(parkingevent);
                    await db.SaveChangesAsync();
                }
                else
                {
                    if (numberOfPlaces < GarageCapacity)
                    {
                        parkingplace.ParkingPlaceId = parkingPlaceId + 1;
                        parkingplace.IsOccupied = true;

                        var parkingevent = new ParkingEvent
                        {
                            ParkingPlace = parkingplace,
                            Vehicle = parkingVehicle,
                            TimeOfArrival = DateTime.Now
                        };
                        db.ParkingEvent.Add(parkingevent);
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        TempData["NoPlacesMessage"] = "There are now no places availible in this garage";
                    }
                }
            }

            return RedirectToAction("MemberDetails", "Owners", new { id, vehcleid = vehicleid });

        }
    }
}
