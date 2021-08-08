namespace SportBattles.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class TeamNameCountryIdUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_CountryId",
                table: "Teams");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CountryId_Name",
                table: "Teams",
                columns: new[] { "CountryId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_CountryId_Name",
                table: "Teams");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CountryId",
                table: "Teams",
                column: "CountryId");
        }
    }
}
