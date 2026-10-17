using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Narrare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConversationIsGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGroup",
                table: "Conversations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGroup",
                table: "Conversations");
        }
    }
}
