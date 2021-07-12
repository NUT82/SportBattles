using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBattles.Data.Migrations
{
    public partial class AddManyGamesToManyMatches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Games_GameId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_GameId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Matches");

            migrationBuilder.CreateTable(
                name: "GameMatch",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "int", nullable: false),
                    MatchesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameMatch", x => new { x.GamesId, x.MatchesId });
                    table.ForeignKey(
                        name: "FK_GameMatch_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameMatch_Matches_MatchesId",
                        column: x => x.MatchesId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameMatch_MatchesId",
                table: "GameMatch",
                column: "MatchesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameMatch");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_GameId",
                table: "Matches",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Games_GameId",
                table: "Matches",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
