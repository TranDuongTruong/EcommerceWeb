using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppMvc.Net.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDiscountRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProduct_Discount_DiscountModelId",
                table: "CategoryProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Discount_DiscountModelId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_DiscountModelId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProduct_DiscountModelId",
                table: "CategoryProduct");

            migrationBuilder.DropColumn(
                name: "DiscountModelId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DiscountModelId",
                table: "CategoryProduct");

            migrationBuilder.CreateTable(
                name: "CategoryDiscount",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryDiscount", x => new { x.CategoryId, x.DiscountId });
                    table.ForeignKey(
                        name: "FK_CategoryDiscount_CategoryProduct_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryDiscount_Discount_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDiscount",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDiscount", x => new { x.ProductId, x.DiscountId });
                    table.ForeignKey(
                        name: "FK_ProductDiscount_Discount_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDiscount_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryDiscount_DiscountId",
                table: "CategoryDiscount",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDiscount_DiscountId",
                table: "ProductDiscount",
                column: "DiscountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryDiscount");

            migrationBuilder.DropTable(
                name: "ProductDiscount");

            migrationBuilder.AddColumn<int>(
                name: "DiscountModelId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountModelId",
                table: "CategoryProduct",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_DiscountModelId",
                table: "Product",
                column: "DiscountModelId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Discount_DiscountModelId",
                table: "Product",
                column: "DiscountModelId",
                principalTable: "Discount",
                principalColumn: "Id");
        }
    }
}
