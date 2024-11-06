using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Studievereniging.Migrations
{
    /// <inheritdoc />
    public partial class Nametosuggestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Suggestions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Suggestions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Suggestions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Suggestions");

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 18, 10, 58, 49, 678, DateTimeKind.Local).AddTicks(3467), new DateTime(2024, 11, 14, 10, 58, 49, 678, DateTimeKind.Local).AddTicks(3474), new DateTime(2024, 11, 16, 10, 58, 49, 678, DateTimeKind.Local).AddTicks(3403) });

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 11, 10, 58, 49, 678, DateTimeKind.Local).AddTicks(3482), new DateTime(2024, 11, 8, 10, 58, 49, 678, DateTimeKind.Local).AddTicks(3484), new DateTime(2024, 11, 11, 10, 58, 49, 678, DateTimeKind.Local).AddTicks(3480) });
        }
    }
}
