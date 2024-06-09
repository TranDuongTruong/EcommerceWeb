using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppMvc.Net.Migrations
{
    /// <inheritdoc />
    public partial class AddSelectedProductsInCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SelectedCartItemsId",
                table: "Cart",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SelectedCartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SelectedShopDiscountCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelectedSystemDiscountCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedCartItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SelectedProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SelectedCartItemsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectedProducts_SelectedCartItems_SelectedCartItemsId",
                        column: x => x.SelectedCartItemsId,
                        principalTable: "SelectedCartItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_SelectedCartItemsId",
                table: "Cart",
                column: "SelectedCartItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedProducts_SelectedCartItemsId",
                table: "SelectedProducts",
                column: "SelectedCartItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_SelectedCartItems_SelectedCartItemsId",
                table: "Cart",
                column: "SelectedCartItemsId",
                principalTable: "SelectedCartItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_SelectedCartItems_SelectedCartItemsId",
                table: "Cart");

            migrationBuilder.DropTable(
                name: "SelectedProducts");

            migrationBuilder.DropTable(
                name: "SelectedCartItems");

            migrationBuilder.DropIndex(
                name: "IX_Cart_SelectedCartItemsId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "SelectedCartItemsId",
                table: "Cart");
        }
    }
}
