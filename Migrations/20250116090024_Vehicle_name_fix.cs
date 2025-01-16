using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VbtEgitimKampiMVC.Migrations
{
    /// <inheritdoc />
    public partial class Vehicle_name_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingLogs_vehicles_VehicleId",
                table: "ParkingLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_Users_UserId",
                table: "vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vehicles",
                table: "vehicles");

            migrationBuilder.RenameTable(
                name: "vehicles",
                newName: "Vehicles");

            migrationBuilder.RenameIndex(
                name: "IX_vehicles_UserId",
                table: "Vehicles",
                newName: "IX_Vehicles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingLogs_Vehicles_VehicleId",
                table: "ParkingLogs",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Users_UserId",
                table: "Vehicles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingLogs_Vehicles_VehicleId",
                table: "ParkingLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Users_UserId",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "vehicles");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_UserId",
                table: "vehicles",
                newName: "IX_vehicles_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vehicles",
                table: "vehicles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingLogs_vehicles_VehicleId",
                table: "ParkingLogs",
                column: "VehicleId",
                principalTable: "vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_Users_UserId",
                table: "vehicles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
