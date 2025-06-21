using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firma.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aktor",
                columns: table => new
                {
                    IdAktor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataUrodzenia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ZdjecieUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aktor", x => x.IdAktor);
                });

            migrationBuilder.CreateTable(
                name: "Gatunek",
                columns: table => new
                {
                    IdGatunek = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gatunek", x => x.IdGatunek);
                });

            migrationBuilder.CreateTable(
                name: "Reżyser",
                columns: table => new
                {
                    IdReżyser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reżyser", x => x.IdReżyser);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "Film",
                columns: table => new
                {
                    IdFilm = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReleaseYear = table.Column<int>(type: "int", nullable: false),
                    IdReżyser = table.Column<int>(type: "int", nullable: false),
                    DurationInMinutes = table.Column<int>(type: "int", nullable: false),
                    PosterUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Film", x => x.IdFilm);
                    table.ForeignKey(
                        name: "FK_Film_Reżyser_IdReżyser",
                        column: x => x.IdReżyser,
                        principalTable: "Reżyser",
                        principalColumn: "IdReżyser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AktorFilm",
                columns: table => new
                {
                    AktorzyIdAktor = table.Column<int>(type: "int", nullable: false),
                    FilmyIdFilm = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AktorFilm", x => new { x.AktorzyIdAktor, x.FilmyIdFilm });
                    table.ForeignKey(
                        name: "FK_AktorFilm_Aktor_AktorzyIdAktor",
                        column: x => x.AktorzyIdAktor,
                        principalTable: "Aktor",
                        principalColumn: "IdAktor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AktorFilm_Film_FilmyIdFilm",
                        column: x => x.FilmyIdFilm,
                        principalTable: "Film",
                        principalColumn: "IdFilm",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmGatunek",
                columns: table => new
                {
                    FilmyIdFilm = table.Column<int>(type: "int", nullable: false),
                    GatunkiIdGatunek = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmGatunek", x => new { x.FilmyIdFilm, x.GatunkiIdGatunek });
                    table.ForeignKey(
                        name: "FK_FilmGatunek_Film_FilmyIdFilm",
                        column: x => x.FilmyIdFilm,
                        principalTable: "Film",
                        principalColumn: "IdFilm",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmGatunek_Gatunek_GatunkiIdGatunek",
                        column: x => x.GatunkiIdGatunek,
                        principalTable: "Gatunek",
                        principalColumn: "IdGatunek",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recenzja",
                columns: table => new
                {
                    IdRecenzja = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ocena = table.Column<int>(type: "int", nullable: false),
                    Tresc = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DataDodania = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdFilm = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recenzja", x => x.IdRecenzja);
                    table.ForeignKey(
                        name: "FK_Recenzja_Film_IdFilm",
                        column: x => x.IdFilm,
                        principalTable: "Film",
                        principalColumn: "IdFilm",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recenzja_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AktorFilm_FilmyIdFilm",
                table: "AktorFilm",
                column: "FilmyIdFilm");

            migrationBuilder.CreateIndex(
                name: "IX_Film_IdReżyser",
                table: "Film",
                column: "IdReżyser");

            migrationBuilder.CreateIndex(
                name: "IX_FilmGatunek_GatunkiIdGatunek",
                table: "FilmGatunek",
                column: "GatunkiIdGatunek");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzja_IdFilm",
                table: "Recenzja",
                column: "IdFilm");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzja_IdUser",
                table: "Recenzja",
                column: "IdUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AktorFilm");

            migrationBuilder.DropTable(
                name: "FilmGatunek");

            migrationBuilder.DropTable(
                name: "Recenzja");

            migrationBuilder.DropTable(
                name: "Aktor");

            migrationBuilder.DropTable(
                name: "Gatunek");

            migrationBuilder.DropTable(
                name: "Film");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Reżyser");
        }
    }
}
