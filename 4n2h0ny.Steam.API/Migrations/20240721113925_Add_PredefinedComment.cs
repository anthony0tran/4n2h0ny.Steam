using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_PredefinedComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PredefinedCommentId",
                table: "Comment",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PredefinedComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CommentString = table.Column<string>(type: "TEXT", nullable: false),
                    LatestCommentedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CommentedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredefinedComment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PredefinedCommentId",
                table: "Comment",
                column: "PredefinedCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_PredefinedComment_PredefinedCommentId",
                table: "Comment",
                column: "PredefinedCommentId",
                principalTable: "PredefinedComment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_PredefinedComment_PredefinedCommentId",
                table: "Comment");

            migrationBuilder.DropTable(
                name: "PredefinedComment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_PredefinedCommentId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "PredefinedCommentId",
                table: "Comment");
        }
    }
}
