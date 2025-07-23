using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICEDT_TamilApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMainActivityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_MainActivityTypes_ActivityTypeId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_MainActivityTypes_MainActivityTypeId",
                table: "Activities");

            migrationBuilder.DropTable(
                name: "MainActivityTypes");

            migrationBuilder.DropIndex(
                name: "IX_Activities_MainActivityTypeId",
                table: "Activities");

            migrationBuilder.AddColumn<int>(
                name: "MainActivityId",
                table: "Activities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MainActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainActivities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_MainActivityId",
                table: "Activities",
                column: "MainActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_MainActivities_ActivityTypeId",
                table: "Activities",
                column: "ActivityTypeId",
                principalTable: "MainActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_MainActivities_MainActivityId",
                table: "Activities",
                column: "MainActivityId",
                principalTable: "MainActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_MainActivities_ActivityTypeId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_MainActivities_MainActivityId",
                table: "Activities");

            migrationBuilder.DropTable(
                name: "MainActivities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_MainActivityId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "MainActivityId",
                table: "Activities");

            migrationBuilder.CreateTable(
                name: "MainActivityTypes",
                columns: table => new
                {
                    MainActivityTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LessonId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainActivityTypes", x => x.MainActivityTypeId);
                    table.ForeignKey(
                        name: "FK_MainActivityTypes_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "LessonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_MainActivityTypeId",
                table: "Activities",
                column: "MainActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MainActivityTypes_LessonId",
                table: "MainActivityTypes",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_MainActivityTypes_ActivityTypeId",
                table: "Activities",
                column: "ActivityTypeId",
                principalTable: "MainActivityTypes",
                principalColumn: "MainActivityTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_MainActivityTypes_MainActivityTypeId",
                table: "Activities",
                column: "MainActivityTypeId",
                principalTable: "MainActivityTypes",
                principalColumn: "MainActivityTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
