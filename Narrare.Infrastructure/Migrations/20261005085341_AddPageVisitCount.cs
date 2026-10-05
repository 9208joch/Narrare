using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Narrare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPageVisitCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VisitCount",
                table: "Pages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitCount",
                table: "Pages");
        }
    }
}
