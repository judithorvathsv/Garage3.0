using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Garage3.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Owner",
                columns: table => new
                {
                    SocialSecurityNumber = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owner", x => x.SocialSecurityNumber);
                });

            migrationBuilder.CreateTable(
                name: "ParkingPlace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsOccupied = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingPlace", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleType",
                columns: table => new
                {
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleType", x => x.VehicleTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VehicleModel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SocialSecurityNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerSocialSecurityNumber = table.Column<string>(type: "nvarchar(13)", nullable: true),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicle_Owner_OwnerSocialSecurityNumber",
                        column: x => x.OwnerSocialSecurityNumber,
                        principalTable: "Owner",
                        principalColumn: "SocialSecurityNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicle_VehicleType_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleType",
                        principalColumn: "VehicleTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParkingEvent",
                columns: table => new
                {
                    ParkingPlaceId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    TimeOfArrival = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingEvent", x => new { x.ParkingPlaceId, x.VehicleId });
                    table.ForeignKey(
                        name: "FK_ParkingEvent_ParkingPlace_ParkingPlaceId",
                        column: x => x.ParkingPlaceId,
                        principalTable: "ParkingPlace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParkingEvent_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Owner",
                columns: new[] { "SocialSecurityNumber", "FirstName", "LastName" },
                values: new object[,]
                {
                    { "600102-1478", "Isaac", "Newton" },
                    { "987654-3210", "Josef", "Jacobsson" },
                    { "345678-9874", "Joel", "Abelin" },
                    { "234567-1234", "Joel", "Josefsson" },
                    { "123456-7891", "James", "Jones" },
                    { "123456-1234", "Adam", "Abelin" },
                    { "690102-7535", "Thomas", "Edison" },
                    { "680102-1595", "Alexander", "Fleming" },
                    { "134679-2587", "joel", "Viklund" },
                    { "660102-4568", "Nicolaus", "Copernicus" },
                    { "650102-1235", "Charles", "Darwin" },
                    { "640102-4561", "Galileo", "Galilei" },
                    { "630102-7894", "Marie", "Curie" },
                    { "620102-4567", "Stephen", "Hawking" },
                    { "610102-1234", "Albert", "Einstein" },
                    { "670102-7895", "Louis", "Pasteur" }
                });

            migrationBuilder.InsertData(
                table: "VehicleType",
                columns: new[] { "VehicleTypeId", "Size", "Type" },
                values: new object[,]
                {
                    { 8, 1, "Kayak" },
                    { 7, 1, "Canoe" },
                    { 6, 9, "Boat" },
                    { 5, 6, "Van" },
                    { 2, 6, "Truck" },
                    { 3, 6, "Bus" },
                    { 1, 3, "Car" },
                    { 9, 9, "Airplane" },
                    { 4, 1, "Motorcycle" },
                    { 10, 9, "Helicopter" }
                });

            migrationBuilder.InsertData(
                table: "Vehicle",
                columns: new[] { "Id", "Brand", "OwnerSocialSecurityNumber", "RegistrationNumber", "SocialSecurityNumber", "VehicleModel", "VehicleTypeId" },
                values: new object[,]
                {
                    { 1, "Chevrolet", null, "ABC-123", "123456-1234", "Silverado", 1 },
                    { 2, "Toyota", null, "BCD-123", "600102-1478", "RAV4", 1 },
                    { 3, "Honda", null, "CDE-456", "600102-1478", "Accord", 1 },
                    { 4, "Ford", null, "DEF-456", "610102-1234", "Explorer", 1 },
                    { 5, "Subaru", null, "EFG-456", "620102-4567", "Impreza", 1 },
                    { 7, "Kia", null, "FGH-789", "630102-7894", "Stinger", 1 },
                    { 8, "Hyundai", null, "GHI-9512", "640102-4561", "Veloster", 1 },
                    { 9, "Nissan", null, "HIJ-7532", "650102-1235", "Versa", 1 },
                    { 10, "Volvo", null, "IJK-456", "123456-1234", "XC40", 1 },
                    { 11, "BMW", null, "JKL-654", "123456-7891", "X5", 1 },
                    { 12, "BMW", null, "KLM-864", "234567-1234", "i3", 1 },
                    { 13, "Honda", null, "LMN-246", "345678-9874", "Civic", 1 },
                    { 14, "Saab", null, "MNO-931", "134679-2587", "AreoX", 1 },
                    { 16, "Yamaha", null, "AAB-123", "987654-3210", "VMAX", 4 },
                    { 15, "Boeing", null, "N12345", "987654-3210", "777", 9 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingEvent_VehicleId",
                table: "ParkingEvent",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_OwnerSocialSecurityNumber",
                table: "Vehicle",
                column: "OwnerSocialSecurityNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_VehicleTypeId",
                table: "Vehicle",
                column: "VehicleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkingEvent");

            migrationBuilder.DropTable(
                name: "ParkingPlace");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "Owner");

            migrationBuilder.DropTable(
                name: "VehicleType");
        }
    }
}
