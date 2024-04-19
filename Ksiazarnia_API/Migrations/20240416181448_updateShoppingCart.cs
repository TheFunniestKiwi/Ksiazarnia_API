using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ksiazarnia_API.Migrations
{
    /// <inheritdoc />
    public partial class updateShoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientSecret",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "PaymentIntent",
                table: "ShoppingCarts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientSecret",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntent",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
