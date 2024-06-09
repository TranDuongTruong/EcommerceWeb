using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppMvc.Net.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDiscountCategoryAndProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryDiscount");

            migrationBuilder.DropTable(
                name: "ProductDiscount");

            migrationBuilder.AddColumn<int>(
                name: "CategoryProductId",
                table: "Discount",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductModelId",
                table: "Discount",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscountCategory",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    DiscountID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCategory", x => new { x.CategoryID, x.DiscountID });
                    table.ForeignKey(
                        name: "FK_DiscountCategory_CategoryProduct_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "CategoryProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountCategory_Discount_DiscountID",
                        column: x => x.DiscountID,
                        principalTable: "Discount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscountProduct",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    DiscountID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountProduct", x => new { x.ProductID, x.DiscountID });
                    table.ForeignKey(
                        name: "FK_DiscountProduct_Discount_DiscountID",
                        column: x => x.DiscountID,
                        principalTable: "Discount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountProduct_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discount_CategoryProductId",
                table: "Discount",
                column: "CategoryProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_ProductModelId",
                table: "Discount",
                column: "ProductModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCategory_DiscountID",
                table: "DiscountCategory",
                column: "DiscountID");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountProduct_DiscountID",
                table: "DiscountProduct",
                column: "DiscountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_CategoryProduct_CategoryProductId",
                table: "Discount",
                column: "CategoryProductId",
                principalTable: "CategoryProduct",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_Product_ProductModelId",
                table: "Discount",
                column: "ProductModelId",
                principalTable: "Product",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discount_CategoryProduct_CategoryProductId",
                table: "Discount");

            migrationBuilder.DropForeignKey(
                name: "FK_Discount_Product_ProductModelId",
                table: "Discount");

            migrationBuilder.DropTable(
                name: "DiscountCategory");

            migrationBuilder.DropTable(
                name: "DiscountProduct");

            migrationBuilder.DropIndex(
                name: "IX_Discount_CategoryProductId",
                table: "Discount");

            migrationBuilder.DropIndex(
                name: "IX_Discount_ProductModelId",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "CategoryProductId",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "ProductModelId",
                table: "Discount");

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
    }
}
