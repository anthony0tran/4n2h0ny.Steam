using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    /// <inheritdoc />
    public partial class PredefinedComments_context : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_PredefinedComment_PredefinedCommentId",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PredefinedComment",
                table: "PredefinedComment");

            migrationBuilder.RenameTable(
                name: "PredefinedComment",
                newName: "PredefinedComments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "PredefinedComments",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PredefinedComments",
                table: "PredefinedComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_PredefinedComments_PredefinedCommentId",
                table: "Comment",
                column: "PredefinedCommentId",
                principalTable: "PredefinedComments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_PredefinedComments_PredefinedCommentId",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PredefinedComments",
                table: "PredefinedComments");

            migrationBuilder.RenameTable(
                name: "PredefinedComments",
                newName: "PredefinedComment");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "PredefinedComment",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PredefinedComment",
                table: "PredefinedComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_PredefinedComment_PredefinedCommentId",
                table: "Comment",
                column: "PredefinedCommentId",
                principalTable: "PredefinedComment",
                principalColumn: "Id");
        }
    }
}
