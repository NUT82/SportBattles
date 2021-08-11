using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBattles.Data.Migrations
{
    public partial class RemoveThreeWayWinner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoWayWinner",
                table: "TennisPredictions");

            migrationBuilder.DropColumn(
                name: "HalfTimeFullTime",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "OverUnderGoals",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "ThreeWayWinner",
                table: "Predictions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TwoWayWinner",
                table: "TennisPredictions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HalfTimeFullTime",
                table: "Predictions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OverUnderGoals",
                table: "Predictions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThreeWayWinner",
                table: "Predictions",
                type: "int",
                nullable: true);
        }
    }
}
