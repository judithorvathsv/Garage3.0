﻿// <auto-generated />
using System;
using Garage3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Garage3.Migrations
{
    [DbContext(typeof(Garage3Context))]
    [Migration("20210908221710_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Garage3.Models.Owner", b =>
                {
                    b.Property<string>("SocialSecurityNumber")
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.HasKey("SocialSecurityNumber");

                    b.ToTable("Owner");

                    b.HasData(
                        new
                        {
                            SocialSecurityNumber = "600102-1478",
                            FirstName = "Isaac",
                            LastName = "Newton"
                        },
                        new
                        {
                            SocialSecurityNumber = "610102-1234",
                            FirstName = "Albert",
                            LastName = "Einstein"
                        },
                        new
                        {
                            SocialSecurityNumber = "620102-4567",
                            FirstName = "Stephen",
                            LastName = "Hawking"
                        },
                        new
                        {
                            SocialSecurityNumber = "630102-7894",
                            FirstName = "Marie",
                            LastName = "Curie"
                        },
                        new
                        {
                            SocialSecurityNumber = "640102-4561",
                            FirstName = "Galileo",
                            LastName = "Galilei"
                        },
                        new
                        {
                            SocialSecurityNumber = "650102-1235",
                            FirstName = "Charles",
                            LastName = "Darwin"
                        },
                        new
                        {
                            SocialSecurityNumber = "660102-4568",
                            FirstName = "Nicolaus",
                            LastName = "Copernicus"
                        },
                        new
                        {
                            SocialSecurityNumber = "670102-7895",
                            FirstName = "Louis",
                            LastName = "Pasteur"
                        },
                        new
                        {
                            SocialSecurityNumber = "680102-1595",
                            FirstName = "Alexander",
                            LastName = "Fleming"
                        },
                        new
                        {
                            SocialSecurityNumber = "690102-7535",
                            FirstName = "Thomas",
                            LastName = "Edison"
                        },
                        new
                        {
                            SocialSecurityNumber = "123456-1234",
                            FirstName = "Adam",
                            LastName = "Abelin"
                        },
                        new
                        {
                            SocialSecurityNumber = "123456-7891",
                            FirstName = "James",
                            LastName = "Jones"
                        },
                        new
                        {
                            SocialSecurityNumber = "134679-2587",
                            FirstName = "joel",
                            LastName = "Viklund"
                        },
                        new
                        {
                            SocialSecurityNumber = "234567-1234",
                            FirstName = "Joel",
                            LastName = "Josefsson"
                        },
                        new
                        {
                            SocialSecurityNumber = "345678-9874",
                            FirstName = "Joel",
                            LastName = "Abelin"
                        },
                        new
                        {
                            SocialSecurityNumber = "987654-3210",
                            FirstName = "Josef",
                            LastName = "Jacobsson"
                        });
                });

            modelBuilder.Entity("Garage3.Models.ParkingEvent", b =>
                {
                    b.Property<int>("ParkingPlaceId")
                        .HasColumnType("int");

                    b.Property<int>("VehicleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeOfArrival")
                        .HasColumnType("datetime2");

                    b.HasKey("ParkingPlaceId", "VehicleId");

                    b.HasIndex("VehicleId");

                    b.ToTable("ParkingEvent");
                });

            modelBuilder.Entity("Garage3.Models.ParkingPlace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsOccupied")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("ParkingPlace");
                });

            modelBuilder.Entity("Garage3.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("OwnerSocialSecurityNumber")
                        .HasColumnType("nvarchar(13)");

                    b.Property<string>("RegistrationNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("SocialSecurityNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VehicleModel")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("VehicleTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OwnerSocialSecurityNumber");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("Vehicle");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Brand = "Chevrolet",
                            RegistrationNumber = "ABC-123",
                            SocialSecurityNumber = "123456-1234",
                            VehicleModel = "Silverado",
                            VehicleTypeId = 1
                        },
                        new
                        {
                            Id = 2,
                            Brand = "Toyota",
                            RegistrationNumber = "BCD-123",
                            SocialSecurityNumber = "600102-1478",
                            VehicleModel = "RAV4",
                            VehicleTypeId = 1
                        },
                        new
                        {
                            Id = 3,
                            Brand = "Honda",
                            RegistrationNumber = "CDE-456",
                            SocialSecurityNumber = "600102-1478",
                            VehicleModel = "Accord",
                            VehicleTypeId = 1
                        },
                        new
                        {
                            Id = 4,
                            Brand = "Ford",
                            RegistrationNumber = "DEF-456",
                            SocialSecurityNumber = "610102-1234",
                            VehicleModel = "Explorer",
                            VehicleTypeId = 1
                        },
                        new
                        {
                            Id = 5,
                            Brand = "Subaru",
                            RegistrationNumber = "EFG-456",
                            SocialSecurityNumber = "620102-4567",
                            VehicleModel = "Impreza",
                            VehicleTypeId = 1
                        },
                        new
                        {
                            Id = 7,
                            Brand = "Kia",
                            RegistrationNumber = "FGH-789",
                            SocialSecurityNumber = "630102-7894",
                            VehicleModel = "Stinger",
                            VehicleTypeId = 1
                        },
                        new
                        {
                            Id = 8,
                            Brand = "Hyundai",
                            RegistrationNumber = "GHI-9512",
                            SocialSecurityNumber = "640102-4561",
                            VehicleModel = "Veloster",
                            VehicleTypeId = 1
                        },
                        new
                        {
                            Id = 9,
                            Brand = "Nissan",
                            RegistrationNumber = "HIJ-7532",
                            SocialSecurityNumber = "650102-1235",
                            VehicleModel = "Versa",
                            VehicleTypeId = 1
                        },
                        new
                        {
                            Id = 10,
                            Brand = "Volvo",
                            RegistrationNumber = "IJK-456",
                            SocialSecurityNumber = "123456-1234",
                            VehicleModel = "XC40",
                            VehicleTypeId = 1
                        },
                        new
                        {
                            Id = 11,
                            Brand = "BMW",
                            RegistrationNumber = "JKL-654",
                            SocialSecurityNumber = "123456-7891",
                            VehicleModel = "X5",
                            VehicleTypeId = 1
                        },
                        new
                        {
                            Id = 12,
                            Brand = "BMW",
                            RegistrationNumber = "KLM-864",
                            SocialSecurityNumber = "234567-1234",
                            VehicleModel = "i3",
                            VehicleTypeId = 1
                        },
                        new
                        {
                            Id = 13,
                            Brand = "Honda",
                            RegistrationNumber = "LMN-246",
                            SocialSecurityNumber = "345678-9874",
                            VehicleModel = "Civic",
                            VehicleTypeId = 1
                        },
                        new
                        {
                            Id = 14,
                            Brand = "Saab",
                            RegistrationNumber = "MNO-931",
                            SocialSecurityNumber = "134679-2587",
                            VehicleModel = "AreoX",
                            VehicleTypeId = 1
                        },
                        new
                        {
                            Id = 15,
                            Brand = "Boeing",
                            RegistrationNumber = "N12345",
                            SocialSecurityNumber = "987654-3210",
                            VehicleModel = "777",
                            VehicleTypeId = 9
                        },
                        new
                        {
                            Id = 16,
                            Brand = "Yamaha",
                            RegistrationNumber = "AAB-123",
                            SocialSecurityNumber = "987654-3210",
                            VehicleModel = "VMAX",
                            VehicleTypeId = 4
                        });
                });

            modelBuilder.Entity("Garage3.Models.VehicleType", b =>
                {
                    b.Property<int>("VehicleTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.HasKey("VehicleTypeId");

                    b.ToTable("VehicleType");

                    b.HasData(
                        new
                        {
                            VehicleTypeId = 1,
                            Size = 3,
                            Type = "Car"
                        },
                        new
                        {
                            VehicleTypeId = 2,
                            Size = 6,
                            Type = "Truck"
                        },
                        new
                        {
                            VehicleTypeId = 3,
                            Size = 6,
                            Type = "Bus"
                        },
                        new
                        {
                            VehicleTypeId = 4,
                            Size = 1,
                            Type = "Motorcycle"
                        },
                        new
                        {
                            VehicleTypeId = 5,
                            Size = 6,
                            Type = "Van"
                        },
                        new
                        {
                            VehicleTypeId = 6,
                            Size = 9,
                            Type = "Boat"
                        },
                        new
                        {
                            VehicleTypeId = 7,
                            Size = 1,
                            Type = "Canoe"
                        },
                        new
                        {
                            VehicleTypeId = 8,
                            Size = 1,
                            Type = "Kayak"
                        },
                        new
                        {
                            VehicleTypeId = 9,
                            Size = 9,
                            Type = "Airplane"
                        },
                        new
                        {
                            VehicleTypeId = 10,
                            Size = 9,
                            Type = "Helicopter"
                        });
                });

            modelBuilder.Entity("Garage3.Models.ParkingEvent", b =>
                {
                    b.HasOne("Garage3.Models.ParkingPlace", "ParkingPlace")
                        .WithMany("ParkingEvents")
                        .HasForeignKey("ParkingPlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Garage3.Models.Vehicle", "Vehicle")
                        .WithMany("ParkingEvents")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParkingPlace");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Garage3.Models.Vehicle", b =>
                {
                    b.HasOne("Garage3.Models.Owner", "Owner")
                        .WithMany("Vehicles")
                        .HasForeignKey("OwnerSocialSecurityNumber");

                    b.HasOne("Garage3.Models.VehicleType", "VehicleType")
                        .WithMany()
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("Garage3.Models.Owner", b =>
                {
                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("Garage3.Models.ParkingPlace", b =>
                {
                    b.Navigation("ParkingEvents");
                });

            modelBuilder.Entity("Garage3.Models.Vehicle", b =>
                {
                    b.Navigation("ParkingEvents");
                });
#pragma warning restore 612, 618
        }
    }
}
