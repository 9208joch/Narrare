using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Narrare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteStyle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteStyles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeadingFontSize = table.Column<int>(type: "int", nullable: false),
                    HeadingBold = table.Column<bool>(type: "bit", nullable: false),
                    HeadingColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeadingMarginBottom = table.Column<int>(type: "int", nullable: false),
                    TextFontSize = table.Column<int>(type: "int", nullable: false),
                    TextColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextLineHeight = table.Column<double>(type: "float", nullable: false),
                    TextMarginBottom = table.Column<int>(type: "int", nullable: false),
                    ImageMaxWidth = table.Column<int>(type: "int", nullable: false),
                    ImageBorderRadius = table.Column<int>(type: "int", nullable: false),
                    LinkColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkBold = table.Column<bool>(type: "bit", nullable: false),
                    LinkUnderline = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteStyles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteStyles");
        }
    }
}
