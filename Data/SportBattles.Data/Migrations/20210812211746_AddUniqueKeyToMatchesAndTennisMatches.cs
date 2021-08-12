using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBattles.Data.Migrations
{
    public partial class AddUniqueKeyToMatchesAndTennisMatches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TennisMatches_HomePlayerId",
                table: "TennisMatches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_HomeTeamId",
                table: "Matches");

            migrationBuilder.CreateIndex(
                name: "IX_TennisMatches_HomePlayerId_AwayPlayerId_StartTime",
                table: "TennisMatches",
                columns: new[] { "HomePlayerId", "AwayPlayerId", "StartTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamId_AwayTeamId_StartTime",
                table: "Matches",
                columns: new[] { "HomeTeamId", "AwayTeamId", "StartTime" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TennisMatches_HomePlayerId_AwayPlayerId_StartTime",
                table: "TennisMatches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_HomeTeamId_AwayTeamId_StartTime",
                table: "Matches");

            migrationBuilder.CreateIndex(
                name: "IX_TennisMatches_HomePlayerId",
                table: "TennisMatches",
                column: "HomePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamId",
                table: "Matches",
                column: "HomeTeamId");
        }
    }
}
