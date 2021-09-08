using Bogus;
using Garage3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Data
{
    public class SeedData
    {
        private static Faker fake;

        internal static async Task InitAsync(IServiceProvider services)
        {
            using (var db = services.GetRequiredService<Garage3Context>())
            {
                if (await db.Owner.AnyAsync()) return;

                fake = new Faker("sv");

                var owners = GetOwners();
                await db.AddRangeAsync(owners);

                var types = GetTypes();
                await db.AddRangeAsync(types);

                // var vehicles = GetVehicles();
                // await db.AddRangeAsync(vehicles);

                await db.SaveChangesAsync();
            }
        }

        private static List<Owner> GetOwners()
        {
            var owners = new List<Owner>();

            for (int i = 0; i < 200; i++)
            {
                var sNumber1 = fake.Random.Number(1000000, 999999);
                var sNumber2 = fake.Random.Number(0, 4);
                var fName = fake.Name.FirstName();
                var lName = fake.Name.LastName();

                var owner = new Owner
                {
                    SocialSecurityNumber = sNumber1 + "-" + sNumber2,
                    FirstName = fName,
                    LastName = lName,
                };
                owners.Add(owner);
            }
            return owners;
        }


        private static List<VehicleType> GetTypes()
        {
            var types = new List<VehicleType>();

            for (int i = 0; i < 200; i++)
            {
                //var vehicleTypeId = fake.UniqueIndex;
                var vType = fake.Random.Words();
                var vSize = fake.Random.Number(1, 3);

                var type = new VehicleType
                {
                    //VehicleTypeId = vehicleTypeId,
                    Type = vType,
                    Size = vSize
                };
                types.Add(type);
            }
            return types;
        }
        
        private static List<Vehicle> GetVehicles()
        {
            var vehicles = new List<Vehicle>();

            for (int i = 0; i < 200; i++)
            {
                //var vehicleId = fake.UniqueIndex;
                var regNumber = fake.Random.Number(1000000, 999999);
                var brand = fake.Commerce.ProductName();
                var model = fake.Commerce.ProductName();   
                /*
                var vehicle = new Vehicle
                {
                    //vehicleId = Id,
                    regNumber = RegistrationNumber,
                    brand = Brand,
                    model = VehicleModel
                };
                vehicles.Add(vehicle);
                */
            }
            return vehicles;
        }
        
    }
}

    