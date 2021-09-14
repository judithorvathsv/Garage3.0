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
using Microsoft.Extensions.Configuration;

namespace Garage3.Controllers
{
    public class OwnersController : Controller
    {

        private readonly Garage3Context db;
        private IConfiguration config;
        private const int GarageCapacity = 20;
        public OwnersController(Garage3Context context, IConfiguration config)
        {
            db = context;
            this.config = config;
        }


        // GET: Owners
        public async Task<IActionResult> Index()
        {

            return View(await db.Owner.ToListAsync());
        }

        // GET: Owners/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var owner = await db.Owner
                .FirstOrDefaultAsync(m => m.OwnerId == id);
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
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
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
        //public async Task<IActionResult> Edit(int id, [Bind("OwnerId,SocialSecurityNumber,FirstName,LastName")] Owner owner)
        public async Task<IActionResult> Edit(int id, Owner owner)
        {
           // id = owner.SocialSecurityNumber;
            if (id != owner.OwnerId)
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
                    //if (!OwnerExists(owner.OwnerId))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            return View(owner);
        }

        // GET: Owners/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var owner = await db.Owner
                .FirstOrDefaultAsync(m => m.OwnerId == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var owner = await db.Owner.FindAsync(id);
            db.Owner.Remove(owner);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MemberDetails(int id, int vehicleid)
        {
            List<VehicleViewModel> _parkingStatus = new List<VehicleViewModel>();
            try
            {
                var owner = await db.Owner
                   .Where(v => v.OwnerId == id)
                   .Select(m => new OwnerDetailsViewModel
                    {
                        Id = id,
                        SocialSecurityNumber = m.SocialSecurityNumber,
                        FullName = m.FirstName + " " + m.LastName,
                    }).FirstOrDefaultAsync();

                var vehicle = await db.Vehicle
                .Where(v => v.OwnerId == id)
                .Join(db.Owner, v => v.Owner.OwnerId, o => o.OwnerId, (v, o) => new { v, o })
                .Select(m => new OwnerDetailsViewModel
                {
                    Id = id,
                    SocialSecurityNumber = m.o.SocialSecurityNumber,
                    FullName = m.o.FirstName + " " + m.o.LastName,
                    VehicleId = m.v.VehicleId,
                }).FirstOrDefaultAsync();

                var vehicles = await db.Vehicle
                    .Where(v => v.OwnerId == id)
                    .Select(m => new VehicleViewModel
                    {
                        VehicleId = m.VehicleId,
                        RegistrationNumber = m.RegistrationNumber,
                        Brand = m.Brand,
                        VehicleType = m.VehicleType,
                        VehicleModel = m.VehicleModel,
                        IsParked = false
                    }).ToListAsync();

                _parkingStatus = await ParkingStatus(vehicles);

                if(_parkingStatus.Count > 0)
                {
                    var model = new OwnerDetailsViewModel
                    {
                        Id = id,
                        VehicleId = vehicle.VehicleId,
                        FullName = vehicle.FullName,
                        SocialSecurityNumber = vehicle.SocialSecurityNumber,
                        Vehicles = _parkingStatus
                    };
                    var amount = FreeParkingPlaces();
                    TempData["AvailiblePlacesMessage"] = $"There are {amount.ToString()} places left in the garage.";
                    return View(model);
                }
                else
                {
                    var model = new OwnerDetailsViewModel
                    {
                        Id = id,
                        FullName = owner.FullName,
                        SocialSecurityNumber = owner.SocialSecurityNumber
                    };
                    var amount = FreeParkingPlaces();
                    TempData["AvailiblePlacesMessage"] = $"There are {amount.ToString()} places left in the garage.";
                    return View(model);
                }
            
            }
            catch (Exception)
            {
                throw;

            }
        }

        private async Task<List<VehicleViewModel>> ParkingStatus(List<VehicleViewModel> vehicles)
        {
            var _status = new List<VehicleViewModel>();

            foreach (var vehi in vehicles)
            {
                var status = await db.ParkingEvent
               .Where(x => x.VehicleId == vehi.VehicleId)
               .Include(p => p.ParkingPlace)
               .FirstOrDefaultAsync(x => x.ParkingPlace.IsOccupied);

                if (status != null)
                {
                    _status.Add(new VehicleViewModel { VehicleId = vehi.VehicleId, RegistrationNumber = vehi.RegistrationNumber, Brand = vehi.Brand, VehicleModel = vehi.VehicleModel, VehicleType = vehi.VehicleType, IsParked = status.ParkingPlace.IsOccupied });
                }
                else
                {
                    _status.Add(new VehicleViewModel { VehicleId = vehi.VehicleId, RegistrationNumber = vehi.RegistrationNumber, Brand = vehi.Brand, VehicleModel = vehi.VehicleModel, VehicleType = vehi.VehicleType, IsParked = vehi.IsParked });
                }

            }
            return _status;
        }

        public int FreeParkingPlaces()
        {
            var freeplaces =  db.ParkingPlace
                .Where(p => p.IsOccupied == true)
                .ToList();


            //var Capacity = new ConfigurationBuilder().AddJsonFile("launchSettings.json").Build().GetSection("Capacity")["maxCapacity"];
            int availibleplaces = GarageCapacity- freeplaces.Count;

            return availibleplaces;
        }


        //private bool OwnerExists(string id)
        //{
        //    return db.Owner.Any(e => e.OwnerId == id);
        //}


        [ActionName("Overview")]
        public IActionResult Overview()
        {
            var listWithEmpty = (from p in db.Owner
                                 join f in db.Vehicle
                                 on p.OwnerId equals f.OwnerId
                                 into j1 from j2 in j1.DefaultIfEmpty() //j1 and j2 only for outer join

                                 group j2 by new
                                 {
                                     p.OwnerId,
                                     p.SocialSecurityNumber,
                                     p.FirstName,
                                     p.LastName
                                 } into gr

                                 select new MemberDetailsViewModel
                                 {
                                     Id = gr.Key.OwnerId,
                                     SocialSecurityNumber = gr.Key.SocialSecurityNumber,
                                     FirstName = gr.Key.FirstName,
                                     LastName = gr.Key.LastName,
                                     FullName = gr.Key.FirstName + " " + gr.Key.LastName,
                                     NumberOfVehicles = gr.Count(t => t.RegistrationNumber != null),
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
