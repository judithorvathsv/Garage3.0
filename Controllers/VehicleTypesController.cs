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
    public class VehicleTypesController : Controller
    {
        private readonly Garage3Context db;

        public VehicleTypesController(Garage3Context context)
        {
            db = context;
        }

        // GET: VehicleTypes
        public async Task<IActionResult> Index()
        {
            return View(await db.VehicleType.ToListAsync());
        }

        // GET: VehicleTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await db.VehicleType
                .FirstOrDefaultAsync(m => m.VehicleTypeId == id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            return View(vehicleType);
        }

        // GET: VehicleTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleTypeId,Type,Size")] VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                db.Add(vehicleType);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleType);
        }

        // GET: VehicleTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await db.VehicleType.FindAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }
            return View(vehicleType);
        }

        // POST: VehicleTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VehicleTypeId,Type,Size")] VehicleType vehicleType)
        {
            id = vehicleType.VehicleTypeId;
            if (id != vehicleType.VehicleTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(vehicleType);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleTypeExists(vehicleType.VehicleTypeId))
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
            return View(vehicleType);
        }

        // GET: VehicleTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await db.VehicleType
                .FirstOrDefaultAsync(m => m.VehicleTypeId == id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            return View(vehicleType);
        }

        // POST: VehicleTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int VehicleTypeId)
        {
            var vehicleType = await db.VehicleType.FindAsync(VehicleTypeId);
            db.VehicleType.Remove(vehicleType);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleTypeExists(int id)
        {
            return db.VehicleType.Any(e => e.VehicleTypeId == id);
        }
    }
}
