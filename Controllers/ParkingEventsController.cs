using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage3.Data;
using Garage3.Models;

namespace Garage3.Controllers
{
    public class ParkingEventsController : Controller
    {
        private readonly Garage3Context db;

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
        public async Task<IActionResult> Park(int id)
        {
            ParkingEvent parkingEvent = new ParkingEvent();
            parkingEvent.VehicleId = id;

            //MakeParkingPlaces();

        //public int ParkingPlaceId { get; set; }
        //public int VehicleId { get; set; }

            parkingEvent.TimeOfArrival = DateTime.Now;
            var freeparkingplaceid = db.ParkingPlace.Any(p => p.IsOccupied).Equals(false);
            //parkingEvent.ParkingPlace.IsOccupied = true;
            parkingEvent.ParkingPlaceId = 1;


            //parkingEvent.ParkingPlace.ParkingPlaceId;
            //parkingEvent.Vehicle.RegistrationNumber = regnumber;
            if (ModelState.IsValid)
            {
                db.Add(parkingEvent);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parkingEvent);
        }

        private void UpdateParkingPlace()
        {

        }

        private void MakeParkingPlaces()
        {
            for (int i = 0; i < 20; i++)
            {
                var parkingplace = new ParkingPlace();
                parkingplace.IsOccupied = false;
                db.Add(parkingplace);
                db.SaveChanges();
            }
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
            return db.ParkingEvent.Any(e => e.ParkingPlace.ParkingPlaceId == id);
        }

        public override bool Equals(object obj)
        {
            return obj is ParkingEventsController controller &&
                   EqualityComparer<Garage3Context>.Default.Equals(db, controller.db);
        }
    }
}
