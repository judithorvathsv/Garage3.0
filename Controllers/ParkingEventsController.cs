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

        public Garage3Context Context => db;

        public ParkingEventsController(Garage3Context context)
        {
            db = context;
        }

        // GET: ParkingEvents
        public async Task<IActionResult> Index()
        {
            return View(await db.ParkingEvent.ToListAsync());
        }

        // GET: ParkingEvents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingEvent = await db.ParkingEvent
                .FirstOrDefaultAsync(m => m.Vehicle.VehicleId == id);
            if (parkingEvent == null)
            {
                return NotFound();
            }

            return View(parkingEvent);
        }

        // GET: ParkingEvents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParkingEvents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]

        public async Task<IActionResult> Park(int id,int vehcleid)
        {

            var vehicle = await db.Vehicle.Where(v => v.VehicleId == vehcleid)
                .Join(db.VehicleType, v => v.VehicleTypeId, t => t.VehicleTypeId, (v,t) => new { v,t })
                .Select(vh => new {  vh.v.VehicleType, vh.t.Size, vh.v })
                .FirstOrDefaultAsync();

            var member = await db.Owner.Where(o => o.OwnerId == id).Select(o => o).FirstOrDefaultAsync();

            var occupiedSpaces = new List<ParkingEvent>();

            var availableSpaces = await db.ParkingPlace.Where(pp => pp.IsOccupied == false)
                                                 .Select(pp => pp.ParkingPlaceId)
                                                 .ToListAsync();

            var parkingVehicle = await db.Vehicle.Where(v => v.VehicleId == vehcleid).FirstOrDefaultAsync();
            var parkingPlace = await db.ParkingPlace.Where(pp => pp.IsOccupied == false).FirstOrDefaultAsync();
            var parkingplace = new ParkingPlace();

            if (parkingPlace != null)
            {
                parkingplace.ParkingPlaceId = parkingPlace.ParkingPlaceId;
                parkingPlace.IsOccupied = true;

            }
            else
            {
                var parkingPlaceId = await db.ParkingPlace.OrderBy(pp => pp.ParkingPlaceId).Select(pp => pp.ParkingPlaceId).LastOrDefaultAsync();
                parkingplace.ParkingPlaceId = parkingPlaceId +1;
                parkingplace.IsOccupied = true;
            }
      

             var parkingevent = new ParkingEvent
            {
                ParkingPlace = parkingplace,
                Vehicle = parkingVehicle,
                TimeOfArrival = DateTime.Now
            };
            //parkingevent.VehicleId = vehcleid;
            //parkingevent.ParkingPlaceId = availableSpaces.First();
            //parkingevent.TimeOfArrival = DateTime.Now;

            //for (int i = 0; i < vehicle.VehicleType.Size; i++)
            //{
            //    occupiedSpaces.Add(new ParkingEvent { VehicleId = vehcleid, ParkingPlaceId = availableSpaces.First() });
            //}

            db.ParkingEvent.Add(parkingevent);
            await db.SaveChangesAsync();


            //foreach (var item in occupiedSpaces)
            //{
            //    db.ParkingEvent.Add(item);
            //    await db.SaveChangesAsync();
            //}
            var model = new OwnerDetailsViewModel
            {
                Id = id,
                FullName = member.FirstName + " " + member.LastName,
                SocialSecurityNumber = member.SocialSecurityNumber,
                IsParked = true,
                VehicleId = parkingVehicle.VehicleId,
                RegistrationNumber = parkingVehicle.RegistrationNumber,
                Brand = parkingVehicle.Brand,
                VehicleModel = parkingVehicle.VehicleModel,
                VehicleType = vehicle.VehicleType.Type
            };
    

            return View("Details", model);

        }

        private void UpdateStatus()
        {

        }

// GET: ParkingEvents/Edit/5
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var parkingEvent = await db.ParkingEvent.FindAsync(id);
    if (parkingEvent == null)
    {
        return NotFound();
    }
    return View(parkingEvent);
}

// POST: ParkingEvents/Edit/
// To protect from overposting attacks, enable the specific properties you want to bind to.
// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("TimeOfArrival")] ParkingEvent parkingEvent)
{
    if (id != parkingEvent.ParkingPlace.ParkingPlaceId)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            db.Update(parkingEvent);
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ParkingEventExists(parkingEvent.ParkingPlace.ParkingPlaceId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction(nameof(Index));
    }
    return View(parkingEvent);
}

// GET: ParkingEvents/Delete/5
public async Task<IActionResult> Delete(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var parkingEvent = await db.ParkingEvent
        .FirstOrDefaultAsync(m => m.ParkingPlace.ParkingPlaceId == id);
    if (parkingEvent == null)
    {
        return NotFound();
    }

    return View(parkingEvent);
}

// POST: ParkingEvents/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var parkingEvent = await db.ParkingEvent.FindAsync(id);
    db.ParkingEvent.Remove(parkingEvent);
    await db.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}

private bool ParkingEventExists(int id)
{
    return db.ParkingEvent.Any(e => e.ParkingPlace.ParkingPlaceId == id);
}
    }
}
