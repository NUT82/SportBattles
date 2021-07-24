using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBattles.Data.Migrations
{
    public partial class GameNameTeamEmblemUrlAndCountryMatchHalfTimeResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Images_EmblemId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_EmblemId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "EmblemId",
                table: "Teams");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmblemUrl",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "AwayGoalsFirstHalf",
                table: "Matches",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "HomeGoalsFirstHalf",
                table: "Matches",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CountryId",
                table: "Teams",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Countries_CountryId",
                table: "Teams",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Countries_CountryId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_CountryId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "EmblemUrl",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "AwayGoalsFirstHalf",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "HomeGoalsFirstHalf",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "EmblemId",
                table: "Teams",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_EmblemId",
                table: "Teams",
                column: "EmblemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Images_EmblemId",
                table: "Teams",
                column: "EmblemId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
