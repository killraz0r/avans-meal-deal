using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvansMealDeal.Infrastructure.Application.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class Casus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Canteens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OffersHotMeals = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canteens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContainsAlcohol = table.Column<bool>(type: "bit", nullable: false),
                    Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealsPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MealPackageType = table.Column<int>(type: "int", nullable: false),
                    PickupDeadline = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CanteenId = table.Column<int>(type: "int", nullable: false),
                    ReservationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealsPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealsPackages_Canteens_CanteenId",
                        column: x => x.CanteenId,
                        principalTable: "Canteens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealPackageItem",
                columns: table => new
                {
                    MealId = table.Column<int>(type: "int", nullable: false),
                    MealPackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPackageItem", x => new { x.MealId, x.MealPackageId });
                    table.ForeignKey(
                        name: "FK_MealPackageItem_MealsPackages_MealPackageId",
                        column: x => x.MealPackageId,
                        principalTable: "MealsPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealPackageItem_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    PlannedPickup = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    MealPackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_MealsPackages_MealPackageId",
                        column: x => x.MealPackageId,
                        principalTable: "MealsPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealPackageItem_MealPackageId",
                table: "MealPackageItem",
                column: "MealPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_MealsPackages_CanteenId",
                table: "MealsPackages",
                column: "CanteenId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_MealPackageId",
                table: "Reservations",
                column: "MealPackageId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealPackageItem");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "MealsPackages");

            migrationBuilder.DropTable(
                name: "Canteens");
        }
    }
}
