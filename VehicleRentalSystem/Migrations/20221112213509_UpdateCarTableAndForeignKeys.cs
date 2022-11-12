using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentalSystem.Migrations
{
    public partial class UpdateCarTableAndForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Locations",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "Cars",
                newName: "RegistrationNumber");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "AirConditioner",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Availability",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CarImageId",
                table: "Cars",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<float>(
                name: "DailyPrice",
                table: "Cars",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Doors",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FuelConsumption",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "FuelTypeId",
                table: "Cars",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Gearbox",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Luggage",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mileage",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ModelId",
                table: "Cars",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Passengers",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CarModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuelTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuelType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CarImageId",
                table: "Cars",
                column: "CarImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_FuelTypeId",
                table: "Cars",
                column: "FuelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ModelId",
                table: "Cars",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarImages_CarImageId",
                table: "Cars",
                column: "CarImageId",
                principalTable: "CarImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarModels_ModelId",
                table: "Cars",
                column: "ModelId",
                principalTable: "CarModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_FuelTypes_FuelTypeId",
                table: "Cars",
                column: "FuelTypeId",
                principalTable: "FuelTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarImages_CarImageId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarModels_ModelId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_FuelTypes_FuelTypeId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "CarModels");

            migrationBuilder.DropTable(
                name: "FuelTypes");

            migrationBuilder.DropIndex(
                name: "IX_Cars_CarImageId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_FuelTypeId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_ModelId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "AirConditioner",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarImageId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "DailyPrice",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Doors",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "FuelConsumption",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "FuelTypeId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Gearbox",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Luggage",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Passengers",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Locations",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "RegistrationNumber",
                table: "Cars",
                newName: "Model");
        }
    }
}
