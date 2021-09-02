using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBattles.Data.Migrations
{
    public partial class AddUniqueKeyToPredictions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TennisPredictions_UserId",
                table: "TennisPredictions");

            migrationBuilder.DropIndex(
                name: "IX_Predictions_UserId",
                table: "Predictions");

            migrationBuilder.CreateIndex(
                name: "IX_TennisPredictions_UserId_TennisMatchId_GameId",
                table: "TennisPredictions",
                columns: new[] { "UserId", "TennisMatchId", "GameId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_UserId_MatchId_GameId",
                table: "Predictions",
                columns: new[] { "UserId", "MatchId", "GameId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TennisPredictions_UserId_TennisMatchId_GameId",
                table: "TennisPredictions");

            migrationBuilder.DropIndex(
                name: "IX_Predictions_UserId_MatchId_GameId",
                table: "Predictions");

            migrationBuilder.CreateIndex(
                name: "IX_TennisPredictions_UserId",
                table: "TennisPredictions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_UserId",
                table: "Predictions",
                column: "UserId");
        }
    }
}
