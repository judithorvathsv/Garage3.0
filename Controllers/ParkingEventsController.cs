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

        public async Task<IActionResult> Park(int id, int vehcleid)
        {

            //var member = await db.Owner.Where(o => o.OwnerId == id).Select(o => o).FirstOrDefaultAsync();
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
                        TempData["NoPlacesMessage"] = "There are no places in this garage";
                    }

                }
            }

            var model = new OwnerDetailsViewModel
            {
                Id = id,
                FullName = parkingVehicle.Owner.FirstName + " " + parkingVehicle.Owner.LastName,
                SocialSecurityNumber = parkingVehicle.Owner.SocialSecurityNumber,
                IsParked = true,
                VehicleId = parkingVehicle.VehicleId,
                RegistrationNumber = parkingVehicle.RegistrationNumber,
                Brand = parkingVehicle.Brand,
                VehicleModel = parkingVehicle.VehicleModel,
                VehicleType = parkingVehicle.VehicleType.Type
            };

            return View("Details", model);

        }

        public async Task<IActionResult> UnPark(int id, int vehcleid)
        {
            var parkingplaceid = await db.ParkingEvent.Where(p => p.VehicleId == vehcleid).Select(p => p.ParkingPlaceId).FirstOrDefaultAsync();
            var parkingVehicle = await db.Vehicle.Include(v => v.VehicleType).Where(v => v.VehicleId == vehcleid).FirstOrDefaultAsync();
            var member = await db.Owner.Where(o => o.OwnerId == id).Select(o => o).FirstOrDefaultAsync();

            var parkingplace = new ParkingPlace
            {
                ParkingPlaceId = parkingplaceid,
                IsOccupied = false
            };

            db.ParkingPlace.Update(parkingplace);
            await db.SaveChangesAsync();


            var model = new OwnerDetailsViewModel
            {
                Id = id,
                FullName = member.FirstName + " " + member.LastName,
                SocialSecurityNumber = member.SocialSecurityNumber,
                IsParked = false,
                VehicleId = parkingVehicle.VehicleId,
                RegistrationNumber = parkingVehicle.RegistrationNumber,
                Brand = parkingVehicle.Brand,
                VehicleModel = parkingVehicle.VehicleModel,
                VehicleType = parkingVehicle.VehicleType.Type
            };

            return View("Details", model);
        }

            db.ParkingEvent.Update(parkingEvent);
            await db.SaveChangesAsync();

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

        public override bool Equals(object obj)
        {
            return obj is ParkingEventsController controller &&
                   EqualityComparer<Garage3Context>.Default.Equals(db, controller.db);
        }
    }
}
