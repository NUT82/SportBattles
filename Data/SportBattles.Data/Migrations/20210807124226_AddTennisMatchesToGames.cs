using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBattles.Data.Migrations
{
    public partial class AddTennisMatchesToGames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SetGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeGames = table.Column<byte>(type: "tinyint", nullable: true),
                    AwayGames = table.Column<byte>(type: "tinyint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetGames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TennisPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TennisPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TennisPlayers_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TennisMatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomePlayerId = table.Column<int>(type: "int", nullable: false),
                    AwayPlayerId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    HomeSets = table.Column<byte>(type: "tinyint", nullable: true),
                    AwaySets = table.Column<byte>(type: "tinyint", nullable: true),
                    TournamentId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TennisMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TennisMatches_TennisPlayers_AwayPlayerId",
                        column: x => x.AwayPlayerId,
                        principalTable: "TennisPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TennisMatches_TennisPlayers_HomePlayerId",
                        column: x => x.HomePlayerId,
                        principalTable: "TennisPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TennisMatches_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameTennisMatch",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false),
                    TennisMatchId = table.Column<int>(type: "int", nullable: false),
                    DoublePoints = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTennisMatch", x => new { x.GameId, x.TennisMatchId });
                    table.ForeignKey(
                        name: "FK_GameTennisMatch_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameTennisMatch_TennisMatches_TennisMatchId",
                        column: x => x.TennisMatchId,
                        principalTable: "TennisMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SetGameTennisMatch",
                columns: table => new
                {
                    SetGamesId = table.Column<int>(type: "int", nullable: false),
                    TennisMatchesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetGameTennisMatch", x => new { x.SetGamesId, x.TennisMatchesId });
                    table.ForeignKey(
                        name: "FK_SetGameTennisMatch_SetGames_SetGamesId",
                        column: x => x.SetGamesId,
                        principalTable: "SetGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SetGameTennisMatch_TennisMatches_TennisMatchesId",
                        column: x => x.TennisMatchesId,
                        principalTable: "TennisMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameTennisMatch_TennisMatchId",
                table: "GameTennisMatch",
                column: "TennisMatchId");

            migrationBuilder.CreateIndex(
                name: "IX_SetGames_IsDeleted",
                table: "SetGames",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SetGameTennisMatch_TennisMatchesId",
                table: "SetGameTennisMatch",
                column: "TennisMatchesId");

            migrationBuilder.CreateIndex(
                name: "IX_TennisMatches_AwayPlayerId",
                table: "TennisMatches",
                column: "AwayPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_TennisMatches_HomePlayerId",
                table: "TennisMatches",
                column: "HomePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_TennisMatches_IsDeleted",
                table: "TennisMatches",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TennisMatches_TournamentId",
                table: "TennisMatches",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TennisPlayers_CountryId",
                table: "TennisPlayers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TennisPlayers_IsDeleted",
                table: "TennisPlayers",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameTennisMatch");

            migrationBuilder.DropTable(
                name: "SetGameTennisMatch");

            migrationBuilder.DropTable(
                name: "SetGames");

            migrationBuilder.DropTable(
                name: "TennisMatches");

            migrationBuilder.DropTable(
                name: "TennisPlayers");
        }
    }
}
