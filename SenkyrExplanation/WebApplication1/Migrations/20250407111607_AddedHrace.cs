using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddedHrace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Hrac1Id",
                table: "PiskvorkyModel",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Hrac2Id",
                table: "PiskvorkyModel",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hrac1Id",
                table: "PiskvorkyModel");

            migrationBuilder.DropColumn(
                name: "Hrac2Id",
                table: "PiskvorkyModel");
        }
    }
}
