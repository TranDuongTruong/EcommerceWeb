using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppMvc.Net.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckoutViewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CheckoutViewModelId",
                table: "CartItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CheckoutViewModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShopDiscountCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SystemDiscountCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckoutViewModels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CheckoutViewModelId",
                table: "CartItem",
                column: "CheckoutViewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_CheckoutViewModels_CheckoutViewModelId",
                table: "CartItem",
                column: "CheckoutViewModelId",
                principalTable: "CheckoutViewModels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_CheckoutViewModels_CheckoutViewModelId",
                table: "CartItem");

            migrationBuilder.DropTable(
                name: "CheckoutViewModels");

            migrationBuilder.DropIndex(
                name: "IX_CartItem_CheckoutViewModelId",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "CheckoutViewModelId",
                table: "CartItem");
        }
    }
}
