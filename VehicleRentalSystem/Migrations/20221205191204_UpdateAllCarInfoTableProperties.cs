using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentalSystem.Migrations
{
    public partial class UpdateAllCarInfoTableProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Gearbox",
                table: "GearboxTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FuelType",
                table: "FuelTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "CarModels",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "Brands",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "GearboxTypes",
                newName: "Gearbox");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "FuelTypes",
                newName: "FuelType");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CarModels",
                newName: "Model");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Brands",
                newName: "Brand");
        }
    }
}
