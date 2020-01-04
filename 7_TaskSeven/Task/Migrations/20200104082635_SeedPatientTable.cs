using Microsoft.EntityFrameworkCore.Migrations;

namespace Task.Migrations
{
    public partial class SeedPatientTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Patient",
                columns: new[] { "Id", "Diagnosis", "Name" },
                values: new object[,]
                {
                    { 1, "pneumonia", "Mark" },
                    { 2, "tuberculosis", "Hamilton" },
                    { 3, "blood cancer", "James" },
                    { 4, "schizophrenia", "Alexceander" },
                    { 5, "pneumonia", "Arthur" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
