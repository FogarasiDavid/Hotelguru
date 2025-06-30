using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelGuru.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class RegisterMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foglalasok_Felhasznalo_VendegId",
                table: "Foglalasok");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Felhasznalo",
                table: "Felhasznalo");

            migrationBuilder.RenameTable(
                name: "Felhasznalo",
                newName: "Felhasznalok");

            migrationBuilder.RenameIndex(
                name: "IX_Felhasznalo_Felhasznalonev",
                table: "Felhasznalok",
                newName: "IX_Felhasznalok_Felhasznalonev");

            migrationBuilder.RenameIndex(
                name: "IX_Felhasznalo_Email",
                table: "Felhasznalok",
                newName: "IX_Felhasznalok_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Felhasznalok",
                table: "Felhasznalok",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Foglalasok_Felhasznalok_VendegId",
                table: "Foglalasok",
                column: "VendegId",
                principalTable: "Felhasznalok",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foglalasok_Felhasznalok_VendegId",
                table: "Foglalasok");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Felhasznalok",
                table: "Felhasznalok");

            migrationBuilder.RenameTable(
                name: "Felhasznalok",
                newName: "Felhasznalo");

            migrationBuilder.RenameIndex(
                name: "IX_Felhasznalok_Felhasznalonev",
                table: "Felhasznalo",
                newName: "IX_Felhasznalo_Felhasznalonev");

            migrationBuilder.RenameIndex(
                name: "IX_Felhasznalok_Email",
                table: "Felhasznalo",
                newName: "IX_Felhasznalo_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Felhasznalo",
                table: "Felhasznalo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Foglalasok_Felhasznalo_VendegId",
                table: "Foglalasok",
                column: "VendegId",
                principalTable: "Felhasznalo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
