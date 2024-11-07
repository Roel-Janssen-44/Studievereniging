using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Studievereniging.Migrations
{
    /// <inheritdoc />
    public partial class suggestauth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Suggestions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 19, 11, 37, 47, 996, DateTimeKind.Local).AddTicks(5210), new DateTime(2024, 11, 15, 11, 37, 47, 996, DateTimeKind.Local).AddTicks(5215), new DateTime(2024, 11, 17, 11, 37, 47, 996, DateTimeKind.Local).AddTicks(5149) });

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 12, 11, 37, 47, 996, DateTimeKind.Local).AddTicks(5222), new DateTime(2024, 11, 9, 11, 37, 47, 996, DateTimeKind.Local).AddTicks(5224), new DateTime(2024, 11, 12, 11, 37, 47, 996, DateTimeKind.Local).AddTicks(5221) });

            migrationBuilder.CreateIndex(
                name: "IX_Suggestions_CreatedById",
                table: "Suggestions",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_Users_CreatedById",
                table: "Suggestions",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_Users_CreatedById",
                table: "Suggestions");

            migrationBuilder.DropIndex(
                name: "IX_Suggestions_CreatedById",
                table: "Suggestions");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Suggestions");

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 18, 12, 26, 55, 147, DateTimeKind.Local).AddTicks(8752), new DateTime(2024, 11, 14, 12, 26, 55, 147, DateTimeKind.Local).AddTicks(8760), new DateTime(2024, 11, 16, 12, 26, 55, 147, DateTimeKind.Local).AddTicks(8654) });

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 11, 12, 26, 55, 147, DateTimeKind.Local).AddTicks(8771), new DateTime(2024, 11, 8, 12, 26, 55, 147, DateTimeKind.Local).AddTicks(8773), new DateTime(2024, 11, 11, 12, 26, 55, 147, DateTimeKind.Local).AddTicks(8769) });
        }
    }
}
