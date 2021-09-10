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
using AutoMapper;
using Bogus;

namespace Garage3.Controllers
{
    public class OwnersController : Controller
    {
        private readonly Garage3Context db;



        public OwnersController(Garage3Context context)
        {
            db = context;

        }

        // GET: Owners
        public async Task<IActionResult> Index()
        {

            return View(await db.Owner.ToListAsync());
        }

        // GET: Owners/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await db.Owner
                .FirstOrDefaultAsync(m => m.SocialSecurityNumber == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterMemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                    var member = new Owner
                    {
                        SocialSecurityNumber = model.SocialSecurityNumber,
                        FirstName = model.FirstName,
                        LastName = model.LastName
                    };
                    db.Add(member);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Register", "Vehicles", new { id = member.OwnerId });
                }
            return View(model);
        }

        // GET: Owners/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await db.Owner.FindAsync(id);
            if (owner == null)
            {
                return NotFound();
            }
            return View(owner);
        }

        // POST: Owners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SocialSecurityNumber,FirstName,LastName")] Owner owner)
        {
            if (id != owner.SocialSecurityNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(owner);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnerExists(owner.SocialSecurityNumber))
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
            return View(owner);
        }

        // GET: Owners/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await db.Owner
                .FirstOrDefaultAsync(m => m.SocialSecurityNumber == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var owner = await db.Owner.FindAsync(id);
            db.Owner.Remove(owner);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Member(int id)
        {
            try
            {
                var owner = await db.Owner.Where(o => o.OwnerId == id).FirstOrDefaultAsync();
                var vehicles = await db.Vehicle
                .Where(v => v.OwnerId == id)
                .Include(v => v.Owner)
                .Select(m => new Vehicle
                {
                    VehicleId = m.VehicleId,
                    RegistrationNumber = m.RegistrationNumber,
                    Brand = m.Brand,
                    VehicleType = m.VehicleType,
                    VehicleModel = m.VehicleModel,
                }).ToListAsync();

                var model = new OwnerDetailsViewModel
                {
                    Id = id,
                    SocialSecurityNumber = owner.SocialSecurityNumber,
                    FullName = $"{owner.FirstName} {owner.LastName}",
                    Vehicles = vehicles
                };
                if (vehicles == null)
                {
                    return NotFound();
                }
                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool OwnerExists(string id)
        {
            return db.Owner.Any(e => e.SocialSecurityNumber == id);
        }


        [ActionName("Overview")]
        public async Task<IActionResult> Overview()
        {

            var listWithEmpty = (from p in db.Owner
                                 join f in db.Vehicle
                                 on p.OwnerId equals f.OwnerId

                                 group p by new
                                 {
                                     p.OwnerId,
                                     p.SocialSecurityNumber,
                                     p.FirstName,
                                     p.LastName
                                 } into gcs

                                 select new MemberDetailsViewModel
                                 {
                                     Id = gcs.Key.OwnerId,
                                     SocialSecurityNumber = gcs.Key.SocialSecurityNumber,
                                     FirstName = gcs.Key.FirstName,
                                     LastName = gcs.Key.LastName,
                                     FullName = gcs.Key.FirstName + " " + gcs.Key.LastName,
                                     NumberOfVehicles = gcs.Count(),
                                 })
                             .ToList()
                             .OrderBy(x => x.FirstName.Substring(0, 3), StringComparer.Ordinal).ToList();

            return View(listWithEmpty);

        }

        public async Task<IActionResult> CheckUnique(string socialsecuritynumber)
        {
            bool ssnExists = await db.Owner.AnyAsync(o => o.SocialSecurityNumber == socialsecuritynumber);

            if (ssnExists) return Json("A user with this social security number aldready exists.");
            
            return Json(true);
        }
    }
}
