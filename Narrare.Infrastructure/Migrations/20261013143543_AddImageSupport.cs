using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Narrare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImageSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UploadedImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StoragePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedByUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadedImages_Users_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImageUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UploadedImageId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: true),
                    CommentId = table.Column<int>(type: "int", nullable: true),
                    IsInGallery = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageUsages", x => x.Id);
                    table.CheckConstraint("CK_ImageUsage_OneTarget", "(\r\n    [PostId] IS NOT NULL AND [CommentId] IS NULL AND [IsInGallery] = 0\r\n)\r\nOR\r\n(\r\n    [PostId] IS NULL AND [CommentId] IS NOT NULL AND [IsInGallery] = 0\r\n)\r\nOR\r\n(\r\n    [PostId] IS NULL AND [CommentId] IS NULL AND [IsInGallery] = 1\r\n)");
                    table.ForeignKey(
                        name: "FK_ImageUsages_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImageUsages_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ImageUsages_UploadedImages_UploadedImageId",
                        column: x => x.UploadedImageId,
                        principalTable: "UploadedImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageUsages_CommentId",
                table: "ImageUsages",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageUsages_PostId",
                table: "ImageUsages",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageUsages_UploadedImageId",
                table: "ImageUsages",
                column: "UploadedImageId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadedImages_UploadedByUserId",
                table: "UploadedImages",
                column: "UploadedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageUsages");

            migrationBuilder.DropTable(
                name: "UploadedImages");
        }
    }
}
