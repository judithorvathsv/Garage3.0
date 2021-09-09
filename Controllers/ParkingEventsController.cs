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
        private readonly Garage3Context _context;

        public ParkingEventsController(Garage3Context context)
        {
            _context = context;
        }

        // GET: ParkingEvents
        public async Task<IActionResult> Index()
        {
            return View(await _context.ParkingEvent.ToListAsync());
        }

        // GET: ParkingEvents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingEvent = await _context.ParkingEvent
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
            var parkingVehicle = await _context.Vehicle.Where(v => v.VehicleId == id).FirstOrDefaultAsync();
            var parkingPlace = await _context.ParkingPlace.Where(pp => pp.IsOccupied == false).FirstOrDefaultAsync();

            parkingPlace.IsOccupied = true;

            var parkingEvent = new ParkingEvent
            {
                ParkingPlace = parkingPlace,
                Vehicle = parkingVehicle,
                TimeOfArrival = DateTime.Now
            };

            _context.ParkingEvent.Update(parkingEvent);
            await _context.SaveChangesAsync();

            return RedirectToAction("Member","Owners", new OwnerDetailsViewModel { Id = id });
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
                    _context.Update(parkingEvent);
                    await _context.SaveChangesAsync();
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

            var parkingEvent = await _context.ParkingEvent
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
            var parkingEvent = await _context.ParkingEvent.FindAsync(id);
            _context.ParkingEvent.Remove(parkingEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingEventExists(int id)
        {
            return _context.ParkingEvent.Any(e => e.ParkingPlace.ParkingPlaceId == id);
        }
    }
}
