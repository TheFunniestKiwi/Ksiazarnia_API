using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ksiazarnia_API.Migrations
{
    /// <inheritdoc />
    public partial class seedGenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ISBN",
                table: "Books",
                newName: "Publisher");

            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Fantastyka" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "Publisher",
                table: "Books",
                newName: "ISBN");
        }
    }
}
