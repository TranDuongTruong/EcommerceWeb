using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppMvc.Net.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCategoryProductInDiscountModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Discount_DiscountModelId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_DiscountModelId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "DiscountModelId",
                table: "Category");

            migrationBuilder.AddColumn<int>(
                name: "DiscountModelId",
                table: "CategoryProduct",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProduct_DiscountModelId",
                table: "CategoryProduct",
                column: "DiscountModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProduct_Discount_DiscountModelId",
                table: "CategoryProduct",
                column: "DiscountModelId",
                principalTable: "Discount",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProduct_Discount_DiscountModelId",
                table: "CategoryProduct");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProduct_DiscountModelId",
                table: "CategoryProduct");

            migrationBuilder.DropColumn(
                name: "DiscountModelId",
                table: "CategoryProduct");

            migrationBuilder.AddColumn<int>(
                name: "DiscountModelId",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_DiscountModelId",
                table: "Category",
                column: "DiscountModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Discount_DiscountModelId",
                table: "Category",
                column: "DiscountModelId",
                principalTable: "Discount",
                principalColumn: "Id");
        }
    }
}
