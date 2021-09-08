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

        // GET: Owners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Owners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SocialSecurityNumber,FirstName,LastName")] Owner owner)
        {
            if (ModelState.IsValid)
            {
                db.Add(owner);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(owner);
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

        public async Task<IActionResult> MemberOverview()
        {
            var listWithEmpty = (from p in db.Owner
                                 join f in db.Vehicle
                                 on p.SocialSecurityNumber equals f.Owner.SocialSecurityNumber into ThisList
                                 from f in ThisList.DefaultIfEmpty()

                                 group p by new
                                 {
                                     p.FirstName,
                                     p.LastName,
                                     p.SocialSecurityNumber
                                 } into gcs
                                 select new
                                 {
                                     FirstName = gcs.Key.FirstName,
                                     LastName = gcs.Key.LastName,
                                     NumberOfVehicles = gcs.Select(x => x).Distinct().Count(),
                                 })
                                .Select(x => new MemberDetailsViewModel()
                                {
                                    FirstName = x.FirstName,
                                    LastName = x.LastName,
                                    FullName = x.FirstName + " " + x.LastName,
                                    NumberOfVehicles = x.NumberOfVehicles
                                });

            var sortedMemberList = listWithEmpty.OrderBy(x => x.FirstName.Substring(0, 1), StringComparer.Ordinal);

            return View(await sortedMemberList.ToListAsync());
        }

        public async Task<IActionResult> Members(string socialsecuritynumber)
        {
            if (socialsecuritynumber == null)
            {
                return NotFound();
            }

            var vehicle = await db.Vehicle.Join(
                db.Owner,
                v => v.Owner.SocialSecurityNumber, m => m.SocialSecurityNumber,
                (v, m) => new { Vehi = v, Memb = m })
                .FirstOrDefaultAsync(m => m.Memb.SocialSecurityNumber == socialsecuritynumber);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View();
        }

        private bool OwnerExists(string id)
        {
            return db.Owner.Any(e => e.SocialSecurityNumber == id);
        }


        [ActionName("MemberOverview")]
        public async Task<IActionResult> MemberOverview()
        {
            var listWithEmpty = (from p in _context.Owner
                                 join f in _context.Vehicle
                                 on p.SocialSecurityNumber equals f.Owner.SocialSecurityNumber into ThisList
                                 from f in ThisList.DefaultIfEmpty()

                                 group p by new
                                 {
                                     p.FirstName,
                                     p.LastName,
                                     p.SocialSecurityNumber
                                 } into gcs
                                 select new
                                 {
                                     FirstName = gcs.Key.FirstName,
                                     LastName = gcs.Key.LastName,
                                     NumberOfVehicles = gcs.Select(x => x).Distinct().Count(),
                                 })
                               .ToList()
                                .Select(x => new Models.ViewModels.MemberDetailsViewModel()
                                {
                                    FirstName = x.FirstName,
                                    LastName = x.LastName,
                                    FullName = x.FirstName + " " + x.LastName,
                                    NumberOfVehicles = x.NumberOfVehicles
                                });

            var sortedMemberList = listWithEmpty.OrderBy(x => x.FirstName.Substring(0, 1), StringComparer.Ordinal);

            return View(sortedMemberList);
        }
    }
}
