using Microsoft.EntityFrameworkCore.Migrations;

namespace PharmaStoreAPI.Core.Migrations
{
    public partial class InitDefaultValuesForMedicineTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MedicineTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Liquid" });

            migrationBuilder.InsertData(
                table: "MedicineTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Tablet" });

            migrationBuilder.InsertData(
                table: "MedicineTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Capsules" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MedicineTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MedicineTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MedicineTypes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
