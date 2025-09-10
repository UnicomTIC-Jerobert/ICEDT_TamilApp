using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICEDT_TamilApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLevelAccessAndBarcodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Levels",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserLevelAccesses",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    LevelId = table.Column<int>(type: "INTEGER", nullable: false),
                    UnlockedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLevelAccesses", x => new { x.UserId, x.LevelId });
                    table.ForeignKey(
                        name: "FK_UserLevelAccesses_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "LevelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLevelAccesses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 1,
                column: "Barcode",
                value: "malalaiyar-nilai");

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 2,
                column: "Barcode",
                value: "siruvar-nilai");

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 3,
                column: "Barcode",
                value: "aandu-01");

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 4,
                column: "Barcode",
                value: "aandu-02");

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 5,
                column: "Barcode",
                value: "aandu-03");

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 6,
                column: "Barcode",
                value: "aandu-04");

            migrationBuilder.UpdateData(
                table: "Levels",
                keyColumn: "LevelId",
                keyValue: 7,
                column: "Barcode",
                value: "aandu-05");

            migrationBuilder.CreateIndex(
                name: "IX_Levels_Barcode",
                table: "Levels",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLevelAccesses_LevelId",
                table: "UserLevelAccesses",
                column: "LevelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLevelAccesses");

            migrationBuilder.DropIndex(
                name: "IX_Levels_Barcode",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Levels");
        }
    }
}
