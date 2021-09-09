using Microsoft.EntityFrameworkCore.Migrations;

namespace Garage3.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Owner_OwnerId1",
                table: "Vehicle");

            migrationBuilder.DropIndex(
                name: "IX_Vehicle_OwnerId1",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "OwnerId1",
                table: "Vehicle");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "Vehicle",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_OwnerId",
                table: "Vehicle",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Owner_OwnerId",
                table: "Vehicle",
                column: "OwnerId",
                principalTable: "Owner",
                principalColumn: "OwnerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Owner_OwnerId",
                table: "Vehicle");

            migrationBuilder.DropIndex(
                name: "IX_Vehicle_OwnerId",
                table: "Vehicle");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId1",
                table: "Vehicle",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_OwnerId1",
                table: "Vehicle",
                column: "OwnerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Owner_OwnerId1",
                table: "Vehicle",
                column: "OwnerId1",
                principalTable: "Owner",
                principalColumn: "OwnerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
