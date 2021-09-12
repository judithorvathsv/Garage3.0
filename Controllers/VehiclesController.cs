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
using System.Data;

namespace Garage3.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly Garage3Context db;

        public VehiclesController(Garage3Context context)
        {
            db = context;
        }

        //Start page where search on reg nr is done
        public IActionResult Index()
        {
            return View();
        }

        // Search for Vehicle
        public async Task<IActionResult> Search(string searchText)
        {
            var exists = db.Vehicle.Any(v => v.RegistrationNumber == searchText);

            if (exists)
            {
                var model = await db.Vehicle.FirstOrDefaultAsync(v => v.RegistrationNumber == searchText);

                return RedirectToAction("Details", new { id = model.VehicleId });
            }
            else
            {
                TempData["Regnumber"] = searchText.ToUpper();

                return View(nameof(Register));
            }
        }




        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await db.Vehicle
                .FirstOrDefaultAsync(m => m.VehicleId == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }




        /*
         public async Task<IActionResult> Overview()
         {
             var model = new OverviewListViewModel();
             model.VehicleTypesSelectList = await GetAllVehicleTypesAsync();

             var allVehicles = db.Vehicle;

             //IQueryable<OverviewViewModel> vehicles = GetOverviewViewModel(allVehicles);
            // string parkedStatusStr = parkedStatus.ToString();
             var vehicles = GetOverviewViewModelAsEnumerable(allVehicles);
            // parkedStatus = ParkingStatus(parkedStatusStr, model, ref vehicles);

            model.VehicleTypesSelectList = await GetAllVehicleTypesAsync();
            model.Overview = vehicles;

             return View("Overview", model);
         }
         */






        public async Task<IActionResult> Overview()
        {
            var model = new OverviewListViewModel();

            var listWithEmpty = (from f in db.Vehicle
                                 join p in db.Owner on f.OwnerId equals p.OwnerId                       
                                 join t in db.ParkingEvent on f.VehicleId equals t.VehicleId            
                                 join ft in db.VehicleType on f.VehicleTypeId equals ft.VehicleTypeId                           
                                 into l from ft in l.DefaultIfEmpty()                                 

                                 select new OverviewViewModel
                                 {
                                     VehicleId = f.VehicleId,
                                     FullName = p.FirstName + " " + p.LastName,
                                     VehicleRegistrationNumber = f.RegistrationNumber,                
                                     VehicleArrivalTime = t.TimeOfArrival,
                                     VehicleParkDuration = t.TimeOfArrival - DateTime.Now,
                                     VehicleType = ft.Type 
                                     

                                 }).Distinct();

            model.Overview = listWithEmpty;
            return View(nameof(Overview), model);
        }






        private async Task<IEnumerable<SelectListItem>> GetVehicleTypesAsync()
        {
            return await db.Vehicle
                        .Select(t => t.VehicleType)
                        .Distinct()
                        .Select(g => new SelectListItem
                        {
                            Text = g.ToString(),
                            Value = g.ToString()
                        })
                        .ToListAsync();
        }
        private async Task<IEnumerable<SelectListItem>> GetAllVehicleTypesAsync()
        {
            return await db.VehicleType.Select(vt => new SelectListItem
            {
                Text = vt.Type,
                Value = vt.VehicleTypeId.ToString()
            }).ToListAsync();
        }


        /*
        public async Task<IActionResult> Filter(OverviewListViewModel viewModel)
        {
            //var filtermodel = new OverviewListViewModel();
            var model = new OverviewListViewModel();
            var vehicles = await db.Vehicle.ToListAsync();

            var result = string.IsNullOrWhiteSpace(viewModel.Regnumber) ?
                           vehicles :
                           vehicles.Where(m => m.RegistrationNumber.StartsWith(viewModel.Regnumber.ToUpper()));

            //  result = viewModel.Types == null ?
            //  result :
            //   result.Where(v => v.VehicleType == viewModel.Types);

            IQueryable<OverviewViewModel> vehi = result.Select(v => new OverviewViewModel
            {
                VehicleId = v.VehicleId,
               //VehicleType = v.VehicleType,
                VehicleRegistrationNumber = v.RegistrationNumber,
                //VehicleArrivalTime = v.TimeOfArrival,
                //VehicleParkDuration = DateTime.Now - v.TimeOfArrival,
                //VehicleParked = false

            }).AsQueryable();

            model.Overview = vehi.AsEnumerable();

            model.VehicleTypesSelectList = await GetVehicleTypesAsync();

            return View("Overview", model);
        }
        */



    
        //For vehicle Overview sorting
        private IEnumerable<OverviewViewModel> GetOverviewViewModelAsEnumerable()
        {
            var list = (from f in db.Vehicle
                        join p in db.Owner on f.OwnerId equals p.OwnerId                       //owner db
                        join t in db.ParkingEvent on f.VehicleId equals t.VehicleId            //parkingevent db
                        join ft in db.VehicleType on f.VehicleTypeId equals ft.VehicleTypeId   //vehicletype db
                       
                        select new OverviewViewModel
                        {
                            VehicleId = f.VehicleId,
                            FullName = p.FirstName + " " + p.LastName,
                            VehicleRegistrationNumber = f.RegistrationNumber,
                            VehicleArrivalTime = t.TimeOfArrival,
                            VehicleParkDuration = t.TimeOfArrival - DateTime.Now,
                            VehicleType = ft.Type,
                            VehicleTypeId = ft.VehicleTypeId
                        });
            return list.AsEnumerable();         
        } 


        //For vehicle Overview filtering regnumber, type
        public async Task<IActionResult> Filter(OverviewListViewModel viewModel)
        {        
            var vehicleAndOwner = GetOverviewViewModelAsEnumerable();         
            var result =  vehicleAndOwner;

            //type: empty 
            if (viewModel.VehicleTypeId == 0)
            {
                result = viewModel.Regnumber == null ? 
                    vehicleAndOwner : vehicleAndOwner.Where(m => m.VehicleRegistrationNumber.StartsWith(viewModel.Regnumber.ToUpper()));
            }

            //type: selected, regnr: empty 
            if (viewModel.Regnumber == null && viewModel.VehicleTypeId != 0)
                result = vehicleAndOwner.Where(m => m.VehicleTypeId == viewModel.VehicleTypeId);


            // tpe: selected, regnr: selected
            if (viewModel.VehicleTypeId != 0 && viewModel.Regnumber != null)
            {
                result = vehicleAndOwner.Where(
                                            m => m.VehicleRegistrationNumber.StartsWith(viewModel.Regnumber.ToUpper())
                                            &&                                       
                                            m.VehicleTypeId == viewModel.VehicleTypeId); 
            }               

            var model = new OverviewListViewModel();
            model.Overview = result.Select(v => new OverviewViewModel
            {
                VehicleId = v.VehicleId,
                FullName = v.FullName,
                VehicleRegistrationNumber = v.VehicleRegistrationNumber,
                VehicleArrivalTime = v.VehicleArrivalTime,
                VehicleParkDuration = v.VehicleArrivalTime - DateTime.Now,
                VehicleType = v.VehicleType
                //VehicleParked = false   
                });
            
            return View("Overview", model);
        }



        [HttpGet, ActionName("OverviewSort")]
        public async Task<IActionResult> OverviewSort(string sortingVehicle)
        {
            //string parkedStatusStr = sortingVehicle.Split(",")[1];
            //sortingVehicle = sortingVehicle.Split(",")[0];

            ViewData["VehicleTypeSorting"] = string.IsNullOrEmpty(sortingVehicle) ? "VehicleTypeSortingDescending" : "";
            ViewData["RegistrationNumberSorting"] = sortingVehicle == "RegistrationNumberSortingAscending" ? "RegistrationNumberSortingDescending" : "RegistrationNumberSortingAscending";
            ViewData["OwnerSorting"] = sortingVehicle == "OwnerSortingAscending" ? "OwnerSortingDescending" : "OwnerSortingAscending";
            ViewData["ArrivalTimeSorting"] = sortingVehicle == "ArrivalTimeSortingAscending" ? "ArrivalTimeSortingDescending" : "ArrivalTimeSortingAscending";
            ViewData["DurationParkedSorting"] = sortingVehicle == "DurationParkedSortingAscending" ? "DurationParkedSortingDescending" : "DurationParkedSortingAscending";

            var model = new OverviewListViewModel();
            //var allVehicles = db.Vehicle;
            //var vehicles = GetOverviewViewModelAsEnumerable(allVehicles);

            //int parkedStatus = 0;
            //parkedStatus = ParkingStatus(parkedStatusStr, model, ref vehicles);

            var vehicleAndMember = GetOverviewViewModelAsEnumerable();

            switch (sortingVehicle)
            {
                case "VehicleTypeSortingAscending":
                    vehicleAndMember = GetOverviewViewModelAsEnumerable().OrderBy(x => x.VehicleType);
                    break;
                case "VehicleTypeSortingDescending":
                    vehicleAndMember = GetOverviewViewModelAsEnumerable().OrderByDescending(x => x.VehicleType);
                    break;
                case "OwnerSortingAscending":
                    vehicleAndMember = GetOverviewViewModelAsEnumerable().OrderBy(x => x.FullName);
                    break;
                case "OwnerSortingDescending":
                    vehicleAndMember = GetOverviewViewModelAsEnumerable().OrderByDescending(x => x.FullName);
                    break;
                case "RegistrationNumberSortingAscending":
                    vehicleAndMember = GetOverviewViewModelAsEnumerable().OrderBy(x => x.VehicleRegistrationNumber);
                    break;
                case "RegistrationNumberSortingDescending":
                    vehicleAndMember = GetOverviewViewModelAsEnumerable().OrderByDescending(x => x.VehicleRegistrationNumber);
                    break;
                case "ArrivalTimeSortingAscending":
                    vehicleAndMember = GetOverviewViewModelAsEnumerable().OrderBy(x => x.VehicleArrivalTime);
                    break;
                case "ArrivalTimeSortingDescending":
                    vehicleAndMember = GetOverviewViewModelAsEnumerable().OrderByDescending(x => x.VehicleArrivalTime);
                    break;
                case "DurationParkedSortingAscending":
                    vehicleAndMember = GetOverviewViewModelAsEnumerable().OrderBy(x => x.VehicleParkDuration.Days);
                    break;
                case "DurationParkedSortingDescending":
                    vehicleAndMember = GetOverviewViewModelAsEnumerable().OrderByDescending(x => x.VehicleParkDuration.Days);
                    break;
                default:
                    vehicleAndMember = GetOverviewViewModelAsEnumerable().OrderBy(x => x.VehicleType);
                    break;
            }

            model.Overview = vehicleAndMember;
            return PartialView(nameof(Overview), model);
        }





        /*
        private int ParkingStatus(string parkedStatusStr, OverviewListViewModel model, ref IEnumerable<OverviewViewModel> vehicles)
        {
            int parkedStatus;
            if (int.TryParse(parkedStatusStr, out parkedStatus))
            {
                if (parkedStatus == 3)
                {
                    model.AllStatus = true;
                    ViewData["ParkedStatus"] = "3";
                }
                else if (parkedStatus == 2)
                {
                    model.UnparkedStatus = true;
                    ViewData["ParkedStatus"] = "2";
                    vehicles = vehicles.Where(u => u.VehicleParked.Equals(false));
                }
                else
                {
                    model.ParkedStatus = true;
                    ViewData["ParkedStatus"] = "1";
                    vehicles = vehicles.Where(u => u.VehicleParked.Equals(true));
                }
            };
            return parkedStatus;
        }
        */



        /*
        private IQueryable<OverviewViewModel> GetOverviewViewModel(IQueryable<Vehicle> allVehicles)
        {
            return allVehicles.Select(v => new OverviewViewModel
            {
                // VehicleParked = v.IsParked,
                VehicleId = v.VehicleId,
                // VehicleType = v.VehicleType,
                VehicleRegistrationNumber = v.RegistrationNumber,
                //VehicleArrivalTime = v.TimeOfArrival,
                //VehicleParkDuration = DateTime.Now - v.TimeOfArrival

            });
        }
        */

        /*
        private IEnumerable<OverviewViewModel> GetOverviewViewModelAsEnumerable(IQueryable<Vehicle> allVehicles)
        {
            return allVehicles.Select(v => new OverviewViewModel
            {
                //VehicleParked = v.IsParked,
                VehicleId = v.VehicleId,
                // VehicleType = v.VehicleType,
                VehicleRegistrationNumber = v.RegistrationNumber,
                //VehicleArrivalTime = v.TimeOfArrival,
                //VehicleParkDuration = DateTime.Now - v.TimeOfArrival

            }).AsEnumerable();
        }
        */

        [HttpGet]
        public async Task<IActionResult> Register(int id)
        {
            var owner = await db.Owner.FindAsync(id);
            if (owner == null) return NotFound();
            else
            {
                var model = new RegisterVehicleViewModel
                {
                    VehicleTypes = await GetAllVehicleTypesAsync(),
                    Id = id,
                    FullName = $"{owner.FirstName} {owner.LastName}"
                };
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVehicleViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool vehicleIsRegistered = await db.Vehicle.AnyAsync(v => v.RegistrationNumber == model.RegistrationNumber);

                if (!vehicleIsRegistered)
                {
                    var vehicle = new Vehicle
                    {
                        RegistrationNumber = model.RegistrationNumber.ToUpper(),
                        VehicleTypeId = model.VehicleTypeId,
                        Brand = model.Brand,
                        VehicleModel = model.VehicleModel,
                        OwnerId = model.Id,

                    };

                    db.Add(vehicle);
                    await db.SaveChangesAsync();
                    TempData["RegMessage"] = "";
                    return RedirectToAction("Details", new { id = vehicle.VehicleId });
                }
                else
                {
                    var existingvehicle = await db.Vehicle.FirstOrDefaultAsync(v => v.RegistrationNumber.Contains(model.RegistrationNumber));
                    TempData["RegMessage"] = "A vehicle with this registration number is already registered!";
                    return RedirectToAction("Details", new { id = existingvehicle.VehicleId });
                }
            }
            else
            {
                return View(model);
            }

        }
        [HttpGet]
        public async Task<IActionResult> ParkRegisteredVehicle(int? id)
        {
            var vehicle = await db.Vehicle.FirstOrDefaultAsync(x => x.VehicleId == id);
            //vehicle.IsParked = true;
            //vehicle.TimeOfArrival = DateTime.Now;

            try
            {
                db.Update(vehicle);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (vehicle.VehicleId != id)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Details", new { id = vehicle.VehicleId });
        }
        public async Task<IActionResult> UnPark(int? vehicleid)
        {
            var vehicle = await db.Vehicle.FirstOrDefaultAsync(x => x.VehicleId == vehicleid);
            //vehicle.IsParked = false;
            var departureTime = DateTime.Now;
            var parkingEvent = await db.ParkingEvent
                .Where(pe => pe.VehicleId == vehicleid)
                .FirstOrDefaultAsync();

            var parkingPlace = await db.ParkingEvent
                .Where(pe => pe.VehicleId == vehicleid)
                .Include(pp => pp.ParkingPlace)
                .Select(pp => pp.ParkingPlace)
                .FirstOrDefaultAsync();

            parkingPlace.IsOccupied = false;
            var arrivalTime = parkingEvent.TimeOfArrival;
            db.Update(parkingPlace);
            db.Remove(parkingEvent);
            db.SaveChanges();

            return RedirectToAction("UnparkResponse", new { vehicleid, departureTime, arrivalTime });
        }


        public async Task<IActionResult> Change(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var vehicle = await db.Vehicle.FindAsync(Id);

            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        public bool Equals(Vehicle b1, Vehicle b2)
        {
            if (b1.RegistrationNumber == b2.RegistrationNumber
                //  &&                 b1.Color == b2.Color 
                && b1.Brand == b2.Brand
                && b1.VehicleModel == b2.VehicleModel
                //&& b1.NumberOfWheels == b2.NumberOfWheels 
                && b1.VehicleType == b2.VehicleType)
                return true;
            else
                return false;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Change(int Id, Vehicle vehicle)
        {
            //från databasen
            var v1 = await db.Vehicle.AsNoTracking().FirstOrDefaultAsync(v => v.VehicleId == Id);

            if (!Equals(v1, vehicle))
            {

                if (Id != vehicle.VehicleId)
                {
                    return NotFound();
                }

                //string str = vehicle.Color;
                vehicle.RegistrationNumber = v1.RegistrationNumber;
                //vehicle.Color = FirstLetterToUpper(str);
                //vehicle.TimeOfArrival = v1.TimeOfArrival;

                if (ModelState.IsValid)
                {
                    try
                    {
                        db.Update(vehicle);
                        await db.SaveChangesAsync();
                        TempData["ChangedVehicle"] = "The vehicle is changed!";
                        return RedirectToAction("Details", new { vehicle.VehicleId });
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!VehicleExists(vehicle.VehicleId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            //return View(vehicle);
            return RedirectToAction("Details", new { vehicle.VehicleId });
        }

        private string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await db.Vehicle
                .FirstOrDefaultAsync(m => m.VehicleId == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await db.Vehicle.FindAsync(id);
            db.Vehicle.Remove(vehicle);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return db.Vehicle.Any(e => e.VehicleId == id);
        }
        public async Task<IActionResult> UnParkResponse(int vehicleId, DateTime departureTime, DateTime arrivalTime)
        {
            //    var v = db.Vehicle
            //.Select(v => v.Id);
            var model = await db.Vehicle
                .Select(v => new UnParkResponseViewModel
                {
                    VehicleId = v.VehicleId,
                    //VehicleType = v.VehicleType.Type,
                    VehicleRegistrationNumber = v.RegistrationNumber,
                    VehicleArrivalTime = arrivalTime,
                    VehicleDepartureTime = departureTime,
                    //VehicleParkDuration=v.
                    //VehicleParkPrice
                })
                .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);

            return View("UnParkResponse", model);
            //ViewBag.id = id;
            //ViewBag.departureTime = departureTime;
            //return View(nameof(UnParkResponse));
        }

        public async Task<IActionResult> Receipt(int vehicleId, DateTime departureTime, DateTime arrivalTime)
        {
            var vehicle = await db.Vehicle
                .Include(v => v.ParkingEvents)
                .Include(v => v.Owner)
                .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);

            var model = new ReceiptViewModel
            {
                VehicleRegistrationNumber = vehicle.RegistrationNumber,
                VehicleArrivalTime = vehicle.ParkingEvents.Select(pe => pe.TimeOfArrival).FirstOrDefault(),
                VehicleDepartureTime = departureTime,
                VehicleParkDuration = arrivalTime - departureTime,
                VehicleParkPrice = (departureTime - arrivalTime).TotalHours * 100,

                MemberFullName = $"{vehicle.Owner.LastName}, {vehicle.Owner.FirstName}",
                MemberSSN = vehicle.Owner.SocialSecurityNumber
            };

            return View(model);
        }



        //private OverviewViewModel OverviewViewModelBuilder(Vehicle vehicle)
        //{
        //    return new OverviewViewModel
        //    {
        //        //VehicleParked = vehicle.IsParked,
        //        VehicleId = vehicle.VehicleId,
        //        //VehicleType = vehicle.VehicleType,
        //        VehicleRegistrationNumber = vehicle.RegistrationNumber,
        //        //VehicleArrivalTime = vehicle.TimeOfArrival,
        //        //VehicleParkDuration = DateTime.Now - vehicle.TimeOfArrival
        //    };
        //}


        //public async Task<IActionResult> Statistics()
        //{
        //    var vehicles = await db.Vehicle.ToListAsync();

        //    var model = new StatisticsViewModel
        //    {
        //        VehicleTypesData = Enum.GetValues(typeof(VehicleTypes))
        //                               .Cast<VehicleTypes>()
        //                               .ToDictionary(type => type.ToString(), type => vehicles
        //                                                                                .Where(v => v.VehicleType == type && v.IsParked)
        //                                                                                .Count()),

        //        NumberOfWheels = vehicles
        //                            .Where(v => v.IsParked)
        //                            .Select(v => v.NumberOfWheels)
        //                            .Sum(),

        //        GeneratedRevenue = vehicles
        //                            .Where(v => v.IsParked)
        //                            .Select(v =>
        //                                (DateTime.Now - v.TimeOfArrival).TotalHours
        //                              + (DateTime.Now - v.TimeOfArrival).TotalDays * 24
        //                                )
        //                            .Sum() * 100
        //    };
        //    return View(model);
        //}

    }
}