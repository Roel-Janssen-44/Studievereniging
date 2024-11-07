using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Studievereniging.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingColumnsToSuggestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 19, 11, 47, 23, 992, DateTimeKind.Local).AddTicks(6248), new DateTime(2024, 11, 15, 11, 47, 23, 992, DateTimeKind.Local).AddTicks(6254), new DateTime(2024, 11, 17, 11, 47, 23, 992, DateTimeKind.Local).AddTicks(6179) });

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "RegistrationDeadline", "StartDate" },
                values: new object[] { new DateTime(2024, 11, 12, 11, 47, 23, 992, DateTimeKind.Local).AddTicks(6264), new DateTime(2024, 11, 9, 11, 47, 23, 992, DateTimeKind.Local).AddTicks(6266), new DateTime(2024, 11, 12, 11, 47, 23, 992, DateTimeKind.Local).AddTicks(6261) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
