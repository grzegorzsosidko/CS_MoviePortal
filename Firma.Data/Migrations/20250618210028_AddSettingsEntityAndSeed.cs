using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Firma.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSettingsEntityAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ustawienie",
                columns: table => new
                {
                    IdUstawienia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Klucz = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Wartosc = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ustawienie", x => x.IdUstawienia);
                });

            migrationBuilder.InsertData(
                table: "Ustawienie",
                columns: new[] { "IdUstawienia", "Klucz", "Wartosc" },
                values: new object[,]
                {
                    { 1, "WelcomeHeadline", "Witaj w MoviePortal!" },
                    { 2, "WelcomeText", "Twoja baza najlepszych filmów." },
                    { 3, "BannerText", "Odkryj świat kina z nami. Przeglądaj, oceniaj i dodawaj recenzje swoich ulubionych filmów." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ustawienie");
        }
    }
}
