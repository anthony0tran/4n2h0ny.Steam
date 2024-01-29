using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_Profile_FetchedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FetchedOn",
                table: "Profiles",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FetchedOn",
                table: "Profiles");
        }
    }
}
