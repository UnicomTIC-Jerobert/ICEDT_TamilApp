using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICEDT_TamilApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePasswordResetToOTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordResetTokenExpiryTime",
                table: "Users",
                newName: "PasswordResetOTPExpiryTime");

            migrationBuilder.RenameColumn(
                name: "PasswordResetToken",
                table: "Users",
                newName: "PasswordResetOTP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordResetOTPExpiryTime",
                table: "Users",
                newName: "PasswordResetTokenExpiryTime");

            migrationBuilder.RenameColumn(
                name: "PasswordResetOTP",
                table: "Users",
                newName: "PasswordResetToken");
        }
    }
}
