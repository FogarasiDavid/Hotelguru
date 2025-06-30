using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelGuru.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class HGDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Felhasznalo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Felhasznalonev = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JelszoHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeljesNev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Telefonszam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lakcim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Felhasznalo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PluszSzolgaltatasok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Leiras = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ar = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PluszSzolgaltatasok", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Szobak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Szobaszam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FerohelyekSzama = table.Column<int>(type: "int", nullable: false),
                    Felszereltseg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foglalhato = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Szobak", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Foglalasok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendegId = table.Column<int>(type: "int", nullable: false),
                    SzobaId = table.Column<int>(type: "int", nullable: false),
                    ErkezesDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TavozasDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Visszaigazolva = table.Column<bool>(type: "bit", nullable: false),
                    LemondasiIdo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foglalasok", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Foglalasok_Felhasznalo_VendegId",
                        column: x => x.VendegId,
                        principalTable: "Felhasznalo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Foglalasok_Szobak_SzobaId",
                        column: x => x.SzobaId,
                        principalTable: "Szobak",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoglalasPluszSzolgaltatas",
                columns: table => new
                {
                    FoglalasokId = table.Column<int>(type: "int", nullable: false),
                    PluszSzolgaltatasokId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoglalasPluszSzolgaltatas", x => new { x.FoglalasokId, x.PluszSzolgaltatasokId });
                    table.ForeignKey(
                        name: "FK_FoglalasPluszSzolgaltatas_Foglalasok_FoglalasokId",
                        column: x => x.FoglalasokId,
                        principalTable: "Foglalasok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoglalasPluszSzolgaltatas_PluszSzolgaltatasok_PluszSzolgaltatasokId",
                        column: x => x.PluszSzolgaltatasokId,
                        principalTable: "PluszSzolgaltatasok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Felhasznalo_Email",
                table: "Felhasznalo",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Felhasznalo_Felhasznalonev",
                table: "Felhasznalo",
                column: "Felhasznalonev",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Foglalasok_SzobaId",
                table: "Foglalasok",
                column: "SzobaId");

            migrationBuilder.CreateIndex(
                name: "IX_Foglalasok_VendegId",
                table: "Foglalasok",
                column: "VendegId");

            migrationBuilder.CreateIndex(
                name: "IX_FoglalasPluszSzolgaltatas_PluszSzolgaltatasokId",
                table: "FoglalasPluszSzolgaltatas",
                column: "PluszSzolgaltatasokId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoglalasPluszSzolgaltatas");

            migrationBuilder.DropTable(
                name: "Foglalasok");

            migrationBuilder.DropTable(
                name: "PluszSzolgaltatasok");

            migrationBuilder.DropTable(
                name: "Felhasznalo");

            migrationBuilder.DropTable(
                name: "Szobak");
        }
    }
}
