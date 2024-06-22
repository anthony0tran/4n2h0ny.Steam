using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    /// <inheritdoc />
    public partial class FetchedIsFriendOn_Profile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FetchedIsFriendOn",
                table: "Profiles",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FetchedIsFriendOn",
                table: "Profiles");
        }
    }
}
