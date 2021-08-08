using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBattles.Data.Migrations
{
    public partial class AddTennisPredictions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TennisPredictions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    TennisMatchId = table.Column<int>(type: "int", nullable: false),
                    HomeSets = table.Column<byte>(type: "tinyint", nullable: true),
                    AwaySets = table.Column<byte>(type: "tinyint", nullable: true),
                    TwoWayWinner = table.Column<int>(type: "int", nullable: true),
                    Points = table.Column<byte>(type: "tinyint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TennisPredictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TennisPredictions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TennisPredictions_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TennisPredictions_TennisMatches_TennisMatchId",
                        column: x => x.TennisMatchId,
                        principalTable: "TennisMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TennisPredictions_GameId",
                table: "TennisPredictions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_TennisPredictions_IsDeleted",
                table: "TennisPredictions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TennisPredictions_TennisMatchId",
                table: "TennisPredictions",
                column: "TennisMatchId");

            migrationBuilder.CreateIndex(
                name: "IX_TennisPredictions_UserId",
                table: "TennisPredictions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TennisPredictions");
        }
    }
}
