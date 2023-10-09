using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocukSubeProject.Migrations
{
    public partial class crime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Crime",
                table: "Suspects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Crime",
                table: "Suspects");
        }
    }
}
