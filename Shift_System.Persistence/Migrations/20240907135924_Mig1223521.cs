using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shift_System.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Mig1223521 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Teams");
        }
    }
}
