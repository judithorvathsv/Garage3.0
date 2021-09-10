using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Garage3.Models;
using Microsoft.Extensions.Logging;

namespace Garage3.Data
{
    public class Garage3Context : DbContext
    {
        public Garage3Context(DbContextOptions<Garage3Context> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<ParkingPlace> ParkingPlace { get; set; }
        public DbSet<Owner> Owner { get; set; }
        public DbSet<VehicleType> VehicleType { get; set; }
        public DbSet<ParkingEvent> ParkingEvent { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.ParkingPlaces)
                .WithMany(pp => pp.Vehicles)
                .UsingEntity<ParkingEvent>(
                pe => pe.HasOne(pe => pe.ParkingPlace).WithMany(v => v.ParkingEvents),
                pe => pe.HasOne(pe => pe.Vehicle).WithMany(pp => pp.ParkingEvents));
            modelBuilder.Entity<ParkingEvent>().HasKey(x => new { x.ParkingPlaceId, x.VehicleId });

//            var owners = modelBuilder.Entity<Owner>().HasData(
//  new Owner { OwnerId = 1, SocialSecurityNumber = "600102-1478", FirstName = "Isaac", LastName = "Newton" },
//  new Owner { OwnerId = 2, SocialSecurityNumber = "610102-1234", FirstName = "Albert", LastName = "Einstein" },
//  new Owner { OwnerId = 3, SocialSecurityNumber = "620102-4567", FirstName = "Stephen", LastName = "Hawking" },
//  new Owner { OwnerId = 4, SocialSecurityNumber = "630102-7894", FirstName = "Marie", LastName = "Curie" },
//  new Owner { OwnerId = 5, SocialSecurityNumber = "640102-4561", FirstName = "Galileo", LastName = "Galilei" },
//  new Owner { OwnerId = 6, SocialSecurityNumber = "650102-1235", FirstName = "Charles", LastName = "Darwin" },
//  new Owner { OwnerId = 7, SocialSecurityNumber = "660102-4568", FirstName = "Nicolaus", LastName = "Copernicus" },
//  new Owner { OwnerId = 8, SocialSecurityNumber = "670102-7895", FirstName = "Louis", LastName = "Pasteur" },
//  new Owner { OwnerId = 9, SocialSecurityNumber = "680102-1595", FirstName = "Alexander", LastName = "Fleming" },
//  new Owner { OwnerId = 10, SocialSecurityNumber = "690102-7535", FirstName = "Thomas", LastName = "Edison" },
//  new Owner { OwnerId = 11, SocialSecurityNumber = "123456-1234", FirstName = "Adam", LastName = "Abelin" },
//  new Owner { OwnerId = 12, SocialSecurityNumber = "123456-7891", FirstName = "James", LastName = "Jones" },
//  new Owner { OwnerId = 13, SocialSecurityNumber = "134679-2587", FirstName = "joel", LastName = "Viklund" },
//  new Owner { OwnerId = 14, SocialSecurityNumber = "234567-1234", FirstName = "Joel", LastName = "Josefsson" },
//  new Owner { OwnerId = 15, SocialSecurityNumber = "345678-9874", FirstName = "Joel", LastName = "Abelin" },
//  new Owner { OwnerId = 16, SocialSecurityNumber = "987654-3210", FirstName = "Josef", LastName = "Jacobsson" }
//              );

//            var vehicleTypes = modelBuilder.Entity<VehicleType>().HasData(
//  new VehicleType { VehicleTypeId = 1, Type = "Car", Size = 3, },
//  new VehicleType { VehicleTypeId = 2, Type = "Truck", Size = 6, },
//  new VehicleType { VehicleTypeId = 3, Type = "Bus", Size = 6, },
//  new VehicleType { VehicleTypeId = 4, Type = "Motorcycle", Size = 1, },
//  new VehicleType { VehicleTypeId = 5, Type = "Van", Size = 6, },
//  new VehicleType { VehicleTypeId = 6, Type = "Boat", Size = 9, },
//  new VehicleType { VehicleTypeId = 7, Type = "Canoe", Size = 1, },
//  new VehicleType { VehicleTypeId = 8, Type = "Kayak", Size = 1, },
//  new VehicleType { VehicleTypeId = 9, Type = "Airplane", Size = 9, },
//  new VehicleType { VehicleTypeId = 10, Type = "Helicopter", Size = 9, }
//  );

//            var vehicles = modelBuilder.Entity<Vehicle>().HasData(
//  new Vehicle { VehicleId = 1, RegistrationNumber = "ABC-123", Brand = "Chevrolet", VehicleModel = "Silverado", OwnerId = 1, VehicleTypeId = 1 },
//  new Vehicle { VehicleId = 2, RegistrationNumber = "BCD-123", Brand = "Toyota", VehicleModel = "RAV4", OwnerId = 2, VehicleTypeId = 1 },
//  new Vehicle { VehicleId = 3, RegistrationNumber = "CDE-456", Brand = "Honda", VehicleModel = "Accord", OwnerId = 3, VehicleTypeId = 1 },
//  new Vehicle { VehicleId = 4, RegistrationNumber = "DEF-456", Brand = "Ford", VehicleModel = "Explorer", OwnerId = 4, VehicleTypeId = 1 },
//  new Vehicle { VehicleId = 5, RegistrationNumber = "EFG-456", Brand = "Subaru", VehicleModel = "Impreza", OwnerId = 5, VehicleTypeId = 1 },
//  new Vehicle { VehicleId = 7, RegistrationNumber = "FGH-789", Brand = "Kia", VehicleModel = "Stinger", OwnerId = 6, VehicleTypeId = 1 },
//  new Vehicle { VehicleId = 8, RegistrationNumber = "GHI-9512", Brand = "Hyundai", VehicleModel = "Veloster", OwnerId = 7, VehicleTypeId = 1 },
//  new Vehicle { VehicleId = 9, RegistrationNumber = "HIJ-7532", Brand = "Nissan", VehicleModel = "Versa", OwnerId = 8, VehicleTypeId = 1 },
//  new Vehicle { VehicleId = 10, RegistrationNumber = "IJK-456", Brand = "Volvo", VehicleModel = "XC40", OwnerId = 9, VehicleTypeId = 1 },
//  new Vehicle { VehicleId = 11, RegistrationNumber = "JKL-654", Brand = "BMW", VehicleModel = "X5", OwnerId = 10, VehicleTypeId = 1 },
//  new Vehicle { VehicleId = 12, RegistrationNumber = "KLM-864", Brand = "BMW", VehicleModel = "i3", OwnerId = 11, VehicleTypeId = 1 },
//  new Vehicle { VehicleId = 13, RegistrationNumber = "LMN-246", Brand = "Honda", VehicleModel = "Civic", OwnerId = 1, VehicleTypeId = 1 },
//  new Vehicle { VehicleId = 14, RegistrationNumber = "MNO-931", Brand = "Saab", VehicleModel = "AreoX", OwnerId = 2, VehicleTypeId = 1 },
//  new Vehicle { VehicleId = 15, RegistrationNumber = "N12345", Brand = "Boeing", VehicleModel = "777", OwnerId = 3, VehicleTypeId = 9 },
//  new Vehicle { VehicleId = 16, RegistrationNumber = "AAB-123", Brand = "Yamaha", VehicleModel = "VMAX", OwnerId = 4, VehicleTypeId = 4 }
//  );

//            var places = modelBuilder.Entity<ParkingPlace>().HasData(
// new ParkingPlace { ParkingPlaceId = 1, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 2, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 3, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 4, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 5, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 6, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 7, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 8, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 9, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 10, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 11, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 12, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 13, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 14, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 15, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 16, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 17, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 18, IsOccupied = true },
// new ParkingPlace { ParkingPlaceId = 19, IsOccupied = true }
//              );


//            var events = modelBuilder.Entity<ParkingEvent>().HasData(
//new ParkingEvent { ParkingPlaceId = 1, VehicleId = 1, TimeOfArrival = DateTime.Now.AddDays(-1) },
//new ParkingEvent { ParkingPlaceId = 2, VehicleId = 1, TimeOfArrival = DateTime.Now.AddDays(-1) },
//new ParkingEvent { ParkingPlaceId = 3, VehicleId = 1, TimeOfArrival = DateTime.Now.AddDays(-1) },

//new ParkingEvent { ParkingPlaceId = 4, VehicleId = 2, TimeOfArrival = DateTime.Now.AddDays(-5) },
//new ParkingEvent { ParkingPlaceId = 5, VehicleId = 2, TimeOfArrival = DateTime.Now.AddDays(-5) },
//new ParkingEvent { ParkingPlaceId = 6, VehicleId = 2, TimeOfArrival = DateTime.Now.AddDays(-5) },

//new ParkingEvent { ParkingPlaceId = 7, VehicleId = 3, TimeOfArrival = DateTime.Now.AddDays(-2) },
//new ParkingEvent { ParkingPlaceId = 8, VehicleId = 3, TimeOfArrival = DateTime.Now.AddDays(-2) },
//new ParkingEvent { ParkingPlaceId = 9, VehicleId = 3, TimeOfArrival = DateTime.Now.AddDays(-2) },

//new ParkingEvent { ParkingPlaceId = 10, VehicleId = 15, TimeOfArrival = DateTime.Now.AddDays(-7) },
//new ParkingEvent { ParkingPlaceId = 11, VehicleId = 15, TimeOfArrival = DateTime.Now.AddDays(-7) },
//new ParkingEvent { ParkingPlaceId = 12, VehicleId = 15, TimeOfArrival = DateTime.Now.AddDays(-7) },
//new ParkingEvent { ParkingPlaceId = 13, VehicleId = 15, TimeOfArrival = DateTime.Now.AddDays(-7) },
//new ParkingEvent { ParkingPlaceId = 14, VehicleId = 15, TimeOfArrival = DateTime.Now.AddDays(-7) },
//new ParkingEvent { ParkingPlaceId = 15, VehicleId = 15, TimeOfArrival = DateTime.Now.AddDays(-7) },
//new ParkingEvent { ParkingPlaceId = 16, VehicleId = 15, TimeOfArrival = DateTime.Now.AddDays(-7) },
//new ParkingEvent { ParkingPlaceId = 17, VehicleId = 15, TimeOfArrival = DateTime.Now.AddDays(-7) },
//new ParkingEvent { ParkingPlaceId = 18, VehicleId = 15, TimeOfArrival = DateTime.Now.AddDays(-7) },

//new ParkingEvent { ParkingPlaceId = 19, VehicleId = 16, TimeOfArrival = DateTime.Now.AddDays(-3) }
//              );
        }
    }
}
