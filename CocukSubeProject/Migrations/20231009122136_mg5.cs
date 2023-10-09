using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocukSubeProject.Migrations
{
    public partial class mg5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duty",
                table: "Suspects");

            migrationBuilder.DropColumn(
                name: "MoneyOnIt",
                table: "Suspects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duty",
                table: "Suspects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MoneyOnIt",
                table: "Suspects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
