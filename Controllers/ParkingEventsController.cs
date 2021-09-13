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
        private const int GarageCaspacity = 20;

        public Garage3Context Context => db;

        public ParkingEventsController(Garage3Context context)
        {
            db = context;
   
        }

        // POST: ParkingEvents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]

        public async Task<IActionResult> Park(int id, int vehcleid)
        {
            var numberOfPlaces = await db.ParkingPlace.AsNoTracking().CountAsync();
            //Hämtar det sista inlagda värdet
            var parkingPlaceId = await db.ParkingPlace.AsNoTracking().OrderBy(pp => pp.ParkingPlaceId).Select(pp => pp.ParkingPlaceId).LastOrDefaultAsync();
            var parkingVehicle = await db.Vehicle.Include(v => v.VehicleType).Where(v => v.VehicleId == vehcleid).FirstOrDefaultAsync();
            var parkingPlace = await db.ParkingPlace.AsNoTracking().Where(pp => pp.IsOccupied == false).FirstOrDefaultAsync();
            var parkingplace = new ParkingPlace();


            if (parkingPlaceId <= GarageCaspacity)
            {
                if (parkingPlace != null)
                {
                    parkingplace.ParkingPlaceId = parkingPlace.ParkingPlaceId;
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
                    if (numberOfPlaces < GarageCaspacity)
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

            return RedirectToAction("MemberDetails", "Owners", new { id = id, vehcleid = vehcleid });

        }

        //public async Task<int> FreeParkingPlaces()
        //{
        //    var freeplaces = db.ParkingPlace
        //        .Where(p => p.IsOccupied == true)
        //        .CountAsync();
        //    int availibleplaces = GarageCaspacity - await freeplaces;

        //    return availibleplaces;
        //}

    //public async Task<IActionResult> UnPark(int id, int vehcleid)
    //{
    //    var parkingplaceId = await db.ParkingEvent.Where(p => p.VehicleId == vehcleid).Select(p => p.ParkingPlaceId).FirstOrDefaultAsync();
    //    var parkingVehicle = await db.Vehicle.Where(v => v.VehicleId == vehcleid).FirstOrDefaultAsync();
    //    var member = await db.Owner.Where(o => o.OwnerId == id).Select(o => o).FirstOrDefaultAsync();

    //    var parkingPlace = new ParkingPlace
    //    {
    //        ParkingPlaceId = parkingplaceId,
    //        IsOccupied = false
    //    };

    //    db.ParkingPlace.Update(parkingPlace);
    //    db.ParkingPlace.Remove(parkingVehicle);
    //    await db.SaveChangesAsync();


    //    var model = new OwnerDetailsViewModel
    //    {
    //        Id = id,
    //        FullName = member.FirstName + " " + member.LastName,
    //        SocialSecurityNumber = member.SocialSecurityNumber,
    //        VehicleId = parkingVehicle.VehicleId
    //    };

    //    return View("Details", model);
    //}
}
}
