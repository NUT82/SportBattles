using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBattles.Data.Migrations
{
    public partial class SportIdToTournament : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SportId",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_SportId",
                table: "Tournaments",
                column: "SportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Sports_SportId",
                table: "Tournaments",
                column: "SportId",
                principalTable: "Sports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Sports_SportId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_SportId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "SportId",
                table: "Tournaments");
        }
    }
}
