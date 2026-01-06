using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    
    public partial class SyncModel_20251105 : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Slots_DoctorId",
                table: "Slots");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_DoctorId_StartAt",
                table: "Slots",
                columns: new[] { "DoctorId", "StartAt" },
                unique: true);
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Slots_DoctorId_StartAt",
                table: "Slots");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_DoctorId",
                table: "Slots",
                column: "DoctorId");
        }
    }
}
