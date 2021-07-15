using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBattles.Data.Migrations
{
    public partial class CodeToCountryAndFlagUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_Images_FlagId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_FlagId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "FlagId",
                table: "Countries");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Countries",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlagUrl",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "FlagUrl",
                table: "Countries");

            migrationBuilder.AddColumn<string>(
                name: "FlagId",
                table: "Countries",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_FlagId",
                table: "Countries",
                column: "FlagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_Images_FlagId",
                table: "Countries",
                column: "FlagId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
