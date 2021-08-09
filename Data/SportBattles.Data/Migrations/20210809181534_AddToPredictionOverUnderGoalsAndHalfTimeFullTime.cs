using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBattles.Data.Migrations
{
    public partial class AddToPredictionOverUnderGoalsAndHalfTimeFullTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HalfTimeFullTime",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "OverUnderGoals",
                table: "Predictions");
        }
    }
}
