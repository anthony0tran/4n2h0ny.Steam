using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_Profile_CommentedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastDateCommented",
                table: "Profiles",
                newName: "LatestCommentReceivedOn");

            migrationBuilder.AddColumn<DateTime>(
                name: "CommentedOn",
                table: "Profiles",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentedOn",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "LatestCommentReceivedOn",
                table: "Profiles",
                newName: "LastDateCommented");
        }
    }
}
