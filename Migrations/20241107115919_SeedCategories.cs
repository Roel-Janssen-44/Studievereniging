using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Studievereniging.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 19, 12, 59, 18, 749, DateTimeKind.Local).AddTicks(7820), new DateTime(2024, 11, 15, 12, 59, 18, 749, DateTimeKind.Local).AddTicks(7820), new DateTime(2024, 11, 17, 12, 59, 18, 749, DateTimeKind.Local).AddTicks(7710) });

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 12, 12, 59, 18, 749, DateTimeKind.Local).AddTicks(7830), new DateTime(2024, 11, 9, 12, 59, 18, 749, DateTimeKind.Local).AddTicks(7830), new DateTime(2024, 11, 12, 12, 59, 18, 749, DateTimeKind.Local).AddTicks(7830) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 19, 12, 58, 16, 1, DateTimeKind.Local).AddTicks(5230), new DateTime(2024, 11, 15, 12, 58, 16, 1, DateTimeKind.Local).AddTicks(5230), new DateTime(2024, 11, 17, 12, 58, 16, 1, DateTimeKind.Local).AddTicks(5170) });

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 12, 12, 58, 16, 1, DateTimeKind.Local).AddTicks(5240), new DateTime(2024, 11, 9, 12, 58, 16, 1, DateTimeKind.Local).AddTicks(5240), new DateTime(2024, 11, 12, 12, 58, 16, 1, DateTimeKind.Local).AddTicks(5240) });
        }
    }
}
