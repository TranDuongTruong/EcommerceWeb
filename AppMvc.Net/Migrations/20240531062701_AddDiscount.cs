using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppMvc.Net.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountModelId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountModelId",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaxUsage = table.Column<int>(type: "int", nullable: false),
                    CurrentUsage = table.Column<int>(type: "int", nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsForAllProducts = table.Column<bool>(type: "bit", nullable: false),
                    AuthorType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discount_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_DiscountModelId",
                table: "Product",
                column: "DiscountModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_DiscountModelId",
                table: "Category",
                column: "DiscountModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_AuthorId",
                table: "Discount",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Discount_DiscountModelId",
                table: "Category",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Discount_DiscountModelId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Discount_DiscountModelId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropIndex(
                name: "IX_Product_DiscountModelId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Category_DiscountModelId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "DiscountModelId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DiscountModelId",
                table: "Category");
        }
    }
}
