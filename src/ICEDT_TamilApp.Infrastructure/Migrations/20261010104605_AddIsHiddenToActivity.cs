using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICEDT_TamilApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsHiddenToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Activities",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Activities");
        }
    }
}
