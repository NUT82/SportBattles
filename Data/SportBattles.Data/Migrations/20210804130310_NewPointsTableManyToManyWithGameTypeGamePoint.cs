using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBattles.Data.Migrations
{
    public partial class NewPointsTableManyToManyWithGameTypeGamePoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "GamePoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePoint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GamePointGameType",
                columns: table => new
                {
                    GamePointsId = table.Column<int>(type: "int", nullable: false),
                    GameTypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePointGameType", x => new { x.GamePointsId, x.GameTypesId });
                    table.ForeignKey(
                        name: "FK_GamePointGameType_GamePoint_GamePointsId",
                        column: x => x.GamePointsId,
                        principalTable: "GamePoint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GamePointGameType_GameTypes_GameTypesId",
                        column: x => x.GameTypesId,
                        principalTable: "GameTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameTypes_Name",
                table: "GameTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GamePoint_IsDeleted",
                table: "GamePoint",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_GamePoint_Name",
                table: "GamePoint",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GamePointGameType_GameTypesId",
                table: "GamePointGameType",
                column: "GameTypesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamePointGameType");

            migrationBuilder.DropTable(
                name: "GamePoint");

            migrationBuilder.DropIndex(
                name: "IX_GameTypes_Name",
                table: "GameTypes");

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
    }
}
