namespace SportBattles.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class PredictionPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Points",
                table: "Predictions",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "Predictions");
        }
    }
}
