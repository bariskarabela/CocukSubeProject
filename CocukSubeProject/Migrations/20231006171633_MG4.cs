using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocukSubeProject.Migrations
{
    public partial class MG4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Suspects",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CatchDate",
                table: "Suspects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CatchDate",
                table: "Suspects");

            migrationBuilder.DropColumn(
                name: "Duty",
                table: "Suspects");

            migrationBuilder.DropColumn(
                name: "MoneyOnIt",
                table: "Suspects");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Suspects",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11,
                oldNullable: true);
        }
    }
}
