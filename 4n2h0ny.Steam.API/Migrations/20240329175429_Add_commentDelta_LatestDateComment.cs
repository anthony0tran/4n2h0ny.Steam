using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_commentDelta_LatestDateComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentDelta",
                table: "ProfileData",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LatestDateCommentOnFetch",
                table: "ProfileData",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDeltaDate",
                table: "ProfileData",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentDelta",
                table: "ProfileData");

            migrationBuilder.DropColumn(
                name: "LatestDateCommentOnFetch",
                table: "ProfileData");

            migrationBuilder.DropColumn(
                name: "StartDeltaDate",
                table: "ProfileData");
        }
    }
}
