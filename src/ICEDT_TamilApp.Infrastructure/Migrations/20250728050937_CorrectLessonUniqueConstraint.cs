using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICEDT_TamilApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrectLessonUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lessons_LevelId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_SequenceOrder",
                table: "Lessons");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_LevelId_SequenceOrder",
                table: "Lessons",
                columns: new[] { "LevelId", "SequenceOrder" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lessons_LevelId_SequenceOrder",
                table: "Lessons");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_LevelId",
                table: "Lessons",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_SequenceOrder",
                table: "Lessons",
                column: "SequenceOrder",
                unique: true);
        }
    }
}
