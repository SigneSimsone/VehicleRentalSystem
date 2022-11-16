using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentalSystem.Migrations
{
    public partial class CreateGearboxTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gearbox",
                table: "Cars");

            migrationBuilder.AddColumn<Guid>(
                name: "GearboxId",
                table: "Cars",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "DriversLicense",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "GearboxModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Gearbox = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GearboxModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_GearboxId",
                table: "Cars",
                column: "GearboxId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_GearboxModel_GearboxId",
                table: "Cars",
                column: "GearboxId",
                principalTable: "GearboxModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_GearboxModel_GearboxId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "GearboxModel");

            migrationBuilder.DropIndex(
                name: "IX_Cars_GearboxId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "GearboxId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "DriversLicense",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Gearbox",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
