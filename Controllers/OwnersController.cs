﻿using System;
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
        public async Task<IActionResult> Edit(string SocialSecurityNumber, [Bind("SocialSecurityNumber,FirstName,LastName")] Owner owner)
        {
           // id = owner.SocialSecurityNumber;
            if (SocialSecurityNumber != owner.SocialSecurityNumber)
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
        public async Task<IActionResult> DeleteConfirmed(string SocialSecurityNumber)
        {
            var owner = await db.Owner.FindAsync(SocialSecurityNumber);
            db.Owner.Remove(owner);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OwnerExists(string id)
        {
            return db.Owner.Any(e => e.SocialSecurityNumber == id);
        }
    }
}
