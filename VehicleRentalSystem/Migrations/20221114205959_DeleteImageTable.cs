using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleRentalSystem.Migrations
{
    public partial class DeleteImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarImages_CarImageId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_GearboxModel_GearboxId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "CarImages");

            migrationBuilder.DropIndex(
                name: "IX_Cars_CarImageId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GearboxModel",
                table: "GearboxModel");

            migrationBuilder.DropColumn(
                name: "CarImageId",
                table: "Cars");

            migrationBuilder.RenameTable(
                name: "GearboxModel",
                newName: "GearboxTypes");

            migrationBuilder.RenameColumn(
                name: "GearboxId",
                table: "Cars",
                newName: "GearboxTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_GearboxId",
                table: "Cars",
                newName: "IX_Cars_GearboxTypeId");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GearboxTypes",
                table: "GearboxTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_GearboxTypes_GearboxTypeId",
                table: "Cars",
                column: "GearboxTypeId",
                principalTable: "GearboxTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_GearboxTypes_GearboxTypeId",
                table: "Cars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GearboxTypes",
                table: "GearboxTypes");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Cars");

            migrationBuilder.RenameTable(
                name: "GearboxTypes",
                newName: "GearboxModel");

            migrationBuilder.RenameColumn(
                name: "GearboxTypeId",
                table: "Cars",
                newName: "GearboxId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_GearboxTypeId",
                table: "Cars",
                newName: "IX_Cars_GearboxId");

            migrationBuilder.AddColumn<Guid>(
                name: "CarImageId",
                table: "Cars",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GearboxModel",
                table: "GearboxModel",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CarImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarImages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CarImageId",
                table: "Cars",
                column: "CarImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarImages_CarImageId",
                table: "Cars",
                column: "CarImageId",
                principalTable: "CarImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_GearboxModel_GearboxId",
                table: "Cars",
                column: "GearboxId",
                principalTable: "GearboxModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
