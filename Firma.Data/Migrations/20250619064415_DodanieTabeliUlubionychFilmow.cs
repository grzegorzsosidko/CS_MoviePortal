using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firma.Data.Migrations
{
    /// <inheritdoc />
    public partial class DodanieTabeliUlubionychFilmow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UlubioneFilmy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUzytkownika = table.Column<int>(type: "int", nullable: false),
                    IdFilmu = table.Column<int>(type: "int", nullable: false),
                    DataDodania = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UlubioneFilmy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UlubioneFilmy_Film_IdFilmu",
                        column: x => x.IdFilmu,
                        principalTable: "Film",
                        principalColumn: "IdFilm",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UlubioneFilmy_User_IdUzytkownika",
                        column: x => x.IdUzytkownika,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UlubioneFilmy_IdFilmu",
                table: "UlubioneFilmy",
                column: "IdFilmu");

            migrationBuilder.CreateIndex(
                name: "IX_UlubioneFilmy_IdUzytkownika_IdFilmu",
                table: "UlubioneFilmy",
                columns: new[] { "IdUzytkownika", "IdFilmu" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UlubioneFilmy");
        }
    }
}
