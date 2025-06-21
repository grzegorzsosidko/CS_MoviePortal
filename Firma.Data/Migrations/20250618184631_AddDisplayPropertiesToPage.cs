using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firma.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDisplayPropertiesToPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Pozycja",
                table: "Strona",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KategoriaStopki",
                table: "Strona",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PokazWNavbar",
                table: "Strona",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KategoriaStopki",
                table: "Strona");

            migrationBuilder.DropColumn(
                name: "PokazWNavbar",
                table: "Strona");

            migrationBuilder.AlterColumn<int>(
                name: "Pozycja",
                table: "Strona",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
