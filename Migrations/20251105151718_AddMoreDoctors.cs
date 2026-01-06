using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreDoctors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorId", "Address", "Email", "Name", "Phone", "Surname" },
                values: new object[,]
                {
                    { 3, null, "marek@clinic.com", "Marek", "500555666", "Lis" },
                    { 4, null, "ewa@clinic.com", "Ewa", "500777888", "Zawisza" },
                    { 5, null, "piotr@clinic.com", "Piotr", "501222333", "Zieliński" },
                    { 6, null, "katarzyna@clinic.com", "Katarzyna", "502333444", "Mazur" },
                    { 7, null, "tomasz@clinic.com", "Tomasz", "503444555", "Wójcik" },
                    { 8, null, "joanna@clinic.com", "Joanna", "504555666", "Piotrowska" },
                    { 9, null, "adam@clinic.com", "Adam", "505666777", "Konieczny" },
                    { 10, null, "magda@clinic.com", "Magdalena", "506777888", "Lewandowska" }
                });

            migrationBuilder.InsertData(
                table: "Specializations",
                columns: new[] { "SpecializationId", "SpecializationName" },
                values: new object[,]
                {
                    { 3, "Laryngolog" },
                    { 4, "Okulista" },
                    { 5, "Neurolog" },
                    { 6, "Urolog" },
                    { 7, "Fizjoterapeuta" }
                });

            migrationBuilder.InsertData(
                table: "DoctorSpecializations",
                columns: new[] { "DoctorId", "SpecializationId" },
                values: new object[,]
                {
                    { 1, 6 },
                    { 1, 7 },
                    { 2, 6 },
                    { 3, 1 },
                    { 4, 3 },
                    { 5, 4 },
                    { 6, 2 },
                    { 7, 5 },
                    { 8, 3 },
                    { 9, 4 },
                    { 10, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 1, 7 });

            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 7, 5 });

            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 8, 3 });

            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 9, 4 });

            migrationBuilder.DeleteData(
                table: "DoctorSpecializations",
                keyColumns: new[] { "DoctorId", "SpecializationId" },
                keyValues: new object[] { 10, 1 });

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "SpecializationId",
                keyValue: 7);

            migrationBuilder.InsertData(
                table: "DoctorSpecializations",
                columns: new[] { "DoctorId", "SpecializationId" },
                values: new object[] { 1, 2 });
        }
    }
}
