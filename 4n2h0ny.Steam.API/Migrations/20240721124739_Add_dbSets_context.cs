using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_dbSets_context : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_PredefinedComments_PredefinedCommentId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentProfile_Comment_CommentsId",
                table: "CommentProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_PredefinedCommentId",
                table: "Comments",
                newName: "IX_Comments_PredefinedCommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentProfile_Comments_CommentsId",
                table: "CommentProfile",
                column: "CommentsId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_PredefinedComments_PredefinedCommentId",
                table: "Comments",
                column: "PredefinedCommentId",
                principalTable: "PredefinedComments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentProfile_Comments_CommentsId",
                table: "CommentProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_PredefinedComments_PredefinedCommentId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PredefinedCommentId",
                table: "Comment",
                newName: "IX_Comment_PredefinedCommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_PredefinedComments_PredefinedCommentId",
                table: "Comment",
                column: "PredefinedCommentId",
                principalTable: "PredefinedComments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentProfile_Comment_CommentsId",
                table: "CommentProfile",
                column: "CommentsId",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
