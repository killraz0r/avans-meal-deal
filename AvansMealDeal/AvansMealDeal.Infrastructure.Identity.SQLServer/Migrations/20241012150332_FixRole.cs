using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AvansMealDeal.Infrastructure.Identity.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class FixRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "108bd950-6071-43dd-8d54-6c159a86e36c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46b83557-511e-457b-8647-256de73f7cdb");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2734a4d7-3135-41b8-8eff-697990624b53", null, "Employee", "EMPLOYEE" },
                    { "d061e63c-f99b-4344-9b59-6ff33bc56bcf", null, "Student", "STUDENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2734a4d7-3135-41b8-8eff-697990624b53");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d061e63c-f99b-4344-9b59-6ff33bc56bcf");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "108bd950-6071-43dd-8d54-6c159a86e36c", null, "Employee", null },
                    { "46b83557-511e-457b-8647-256de73f7cdb", null, "Student", null }
                });
        }
    }
}
