using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AvansMealDeal.Infrastructure.Application.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class Canteens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Canteens",
                columns: new[] { "Id", "Address", "City", "OffersHotMeals" },
                values: new object[,]
                {
                    { 1, "Hogeschoollaan 1", 1, true },
                    { 2, "Lovensdijkstraat 61", 1, false },
                    { 3, "Professor Cobbenhagenlaan 1", 2, true },
                    { 4, "Professor Cobbenhagenlaan 13", 2, false },
                    { 5, "Onderwijsboulevard 215", 3, true },
                    { 6, "Parallelweg 23", 3, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Canteens",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
