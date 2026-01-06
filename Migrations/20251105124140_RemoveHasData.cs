using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHasData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Slots",
                keyColumn: "SlotId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Slots",
                keyColumn: "SlotId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Slots",
                keyColumn: "SlotId",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Slots",
                columns: new[] { "SlotId", "DoctorId", "EndAt", "StartAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 11, 10, 9, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 10, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 1, new DateTime(2025, 11, 10, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 10, 9, 30, 0, 0, DateTimeKind.Utc) },
                    { 3, 2, new DateTime(2025, 11, 10, 9, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 10, 9, 0, 0, 0, DateTimeKind.Utc) }
                });
        }
    }
}
