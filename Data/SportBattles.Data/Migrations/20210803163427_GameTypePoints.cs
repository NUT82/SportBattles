using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBattles.Data.Migrations
{
    public partial class GameTypePoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "ExactScorelinePoints",
                table: "GameTypes",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "GoalDifferencePoints",
                table: "GameTypes",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "OneTeamGoalsPoints",
                table: "GameTypes",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "OutcomePoints",
                table: "GameTypes",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExactScorelinePoints",
                table: "GameTypes");

            migrationBuilder.DropColumn(
                name: "GoalDifferencePoints",
                table: "GameTypes");

            migrationBuilder.DropColumn(
                name: "OneTeamGoalsPoints",
                table: "GameTypes");

            migrationBuilder.DropColumn(
                name: "OutcomePoints",
                table: "GameTypes");
        }
    }
}
