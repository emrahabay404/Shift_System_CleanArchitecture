using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shift_System.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Mig1223 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url1",
                table: "ShiftLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url2",
                table: "ShiftLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url3",
                table: "ShiftLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url4",
                table: "ShiftLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url5",
                table: "ShiftLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url1",
                table: "ShiftLists");

            migrationBuilder.DropColumn(
                name: "Url2",
                table: "ShiftLists");

            migrationBuilder.DropColumn(
                name: "Url3",
                table: "ShiftLists");

            migrationBuilder.DropColumn(
                name: "Url4",
                table: "ShiftLists");

            migrationBuilder.DropColumn(
                name: "Url5",
                table: "ShiftLists");
        }
    }
}
