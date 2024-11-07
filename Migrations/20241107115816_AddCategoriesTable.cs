using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Studievereniging.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Social" },
                    { 2, "Education" },
                    { 3, "Sport" },
                    { 4, "Culture" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

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
