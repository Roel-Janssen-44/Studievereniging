using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Studievereniging.Migrations
{
    /// <inheritdoc />
    public partial class dbfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 18, 10, 43, 1, 10, DateTimeKind.Local).AddTicks(9457), new DateTime(2024, 11, 14, 10, 43, 1, 10, DateTimeKind.Local).AddTicks(9462), new DateTime(2024, 11, 16, 10, 43, 1, 10, DateTimeKind.Local).AddTicks(9409) });

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 11, 10, 43, 1, 10, DateTimeKind.Local).AddTicks(9469), new DateTime(2024, 11, 8, 10, 43, 1, 10, DateTimeKind.Local).AddTicks(9471), new DateTime(2024, 11, 11, 10, 43, 1, 10, DateTimeKind.Local).AddTicks(9468) });
        }
    }
}
