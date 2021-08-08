using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBattles.Data.Migrations
{
    public partial class AddTennisPlayerUniqueNameCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TennisPlayers_CountryId",
                table: "TennisPlayers");

            migrationBuilder.CreateIndex(
                name: "IX_TennisPlayers_CountryId_Name",
                table: "TennisPlayers",
                columns: new[] { "CountryId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TennisPlayers_CountryId_Name",
                table: "TennisPlayers");

            migrationBuilder.CreateIndex(
                name: "IX_TennisPlayers_CountryId",
                table: "TennisPlayers",
                column: "CountryId");
        }
    }
}
