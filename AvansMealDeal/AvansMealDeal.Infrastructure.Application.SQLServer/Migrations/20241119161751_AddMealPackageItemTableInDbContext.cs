using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvansMealDeal.Infrastructure.Application.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class AddMealPackageItemTableInDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPackageItem_MealsPackages_MealPackageId",
                table: "MealPackageItem");

            migrationBuilder.DropForeignKey(
                name: "FK_MealPackageItem_Meals_MealId",
                table: "MealPackageItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealPackageItem",
                table: "MealPackageItem");

            migrationBuilder.RenameTable(
                name: "MealPackageItem",
                newName: "MealPackageItems");

            migrationBuilder.RenameIndex(
                name: "IX_MealPackageItem_MealPackageId",
                table: "MealPackageItems",
                newName: "IX_MealPackageItems_MealPackageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealPackageItems",
                table: "MealPackageItems",
                columns: new[] { "MealId", "MealPackageId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MealPackageItems_MealsPackages_MealPackageId",
                table: "MealPackageItems",
                column: "MealPackageId",
                principalTable: "MealsPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealPackageItems_Meals_MealId",
                table: "MealPackageItems",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPackageItems_MealsPackages_MealPackageId",
                table: "MealPackageItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MealPackageItems_Meals_MealId",
                table: "MealPackageItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealPackageItems",
                table: "MealPackageItems");

            migrationBuilder.RenameTable(
                name: "MealPackageItems",
                newName: "MealPackageItem");

            migrationBuilder.RenameIndex(
                name: "IX_MealPackageItems_MealPackageId",
                table: "MealPackageItem",
                newName: "IX_MealPackageItem_MealPackageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealPackageItem",
                table: "MealPackageItem",
                columns: new[] { "MealId", "MealPackageId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MealPackageItem_MealsPackages_MealPackageId",
                table: "MealPackageItem",
                column: "MealPackageId",
                principalTable: "MealsPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealPackageItem_Meals_MealId",
                table: "MealPackageItem",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
