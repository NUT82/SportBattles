using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBattles.Data.Migrations
{
    public partial class AddTableGamePointGameTypeManyToManyWithValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePointGameType_GamePoint_GamePointsId",
                table: "GamePointGameType");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePointGameType_GameTypes_GameTypesId",
                table: "GamePointGameType");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "GamePoint");

            migrationBuilder.RenameColumn(
                name: "GameTypesId",
                table: "GamePointGameType",
                newName: "GameTypeId");

            migrationBuilder.RenameColumn(
                name: "GamePointsId",
                table: "GamePointGameType",
                newName: "GamePointId");

            migrationBuilder.RenameIndex(
                name: "IX_GamePointGameType_GameTypesId",
                table: "GamePointGameType",
                newName: "IX_GamePointGameType_GameTypeId");

            migrationBuilder.AddColumn<byte>(
                name: "Value",
                table: "GamePointGameType",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePointGameType_GamePoint_GamePointId",
                table: "GamePointGameType",
                column: "GamePointId",
                principalTable: "GamePoint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePointGameType_GameTypes_GameTypeId",
                table: "GamePointGameType",
                column: "GameTypeId",
                principalTable: "GameTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePointGameType_GamePoint_GamePointId",
                table: "GamePointGameType");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePointGameType_GameTypes_GameTypeId",
                table: "GamePointGameType");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "GamePointGameType");

            migrationBuilder.RenameColumn(
                name: "GameTypeId",
                table: "GamePointGameType",
                newName: "GameTypesId");

            migrationBuilder.RenameColumn(
                name: "GamePointId",
                table: "GamePointGameType",
                newName: "GamePointsId");

            migrationBuilder.RenameIndex(
                name: "IX_GamePointGameType_GameTypeId",
                table: "GamePointGameType",
                newName: "IX_GamePointGameType_GameTypesId");

            migrationBuilder.AddColumn<byte>(
                name: "Value",
                table: "GamePoint",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePointGameType_GamePoint_GamePointsId",
                table: "GamePointGameType",
                column: "GamePointsId",
                principalTable: "GamePoint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePointGameType_GameTypes_GameTypesId",
                table: "GamePointGameType",
                column: "GameTypesId",
                principalTable: "GameTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
