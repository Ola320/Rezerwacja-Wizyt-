using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 

namespace WebApplication1.Migrations
{
    
    public partial class SeedData : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndAT",
                table: "Slots",
                newName: "EndAt");

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorId", "Address", "Email", "Name", "Phone", "Surname" },
                values: new object[,]
                {
                    { 1, null, "jan@clinic.com", "Jan", "500111222", "Kowalski" },
                    { 2, null, "anna@clinic.com", "Anna", "500333444", "Nowak" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "Address", "Email", "Name", "Phone", "Surname" },
                values: new object[,]
                {
                    { 1, "Warszawa", "ola@ex.com", "Ola", "501000999", "Olszewska" },
                    { 2, "Kraków", "piotr@ex.com", "Piotr", "501000888", "Zieliński" }
                });

            migrationBuilder.InsertData(
                table: "Specializations",
                columns: new[] { "SpecializationId", "SpecializationName" },
                values: new object[,]
                {
                    { 1, "Kardiolog" },
                    { 2, "Dermatolog" }
                });

            migrationBuilder.InsertData(
                table: "DoctorSpecializations",
                columns: new[] { "DoctorId", "SpecializationId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 2 }
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 2);

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

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 2);

            migrationBuilder.RenameColumn(
                name: "EndAt",
                table: "Slots",
                newName: "EndAT");
        }
    }
}
