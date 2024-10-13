using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shift_System.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Mig567 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableId",
                table: "DocumentInfos");

            migrationBuilder.AddColumn<string>(
                name: "TableName",
                table: "DocumentInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableName",
                table: "DocumentInfos");

            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "DocumentInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
