using Microsoft.EntityFrameworkCore.Migrations;

namespace Task_3.Migrations
{
    public partial class AddField_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Field_1",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Field_1",
                table: "Users");
        }
    }
}
