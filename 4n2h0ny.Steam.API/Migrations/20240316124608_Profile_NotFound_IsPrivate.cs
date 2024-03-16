using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    /// <inheritdoc />
    public partial class Profile_NotFound_IsPrivate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileNotFound",
                table: "Profiles",
                newName: "NotFound");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Profiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "NotFound",
                table: "Profiles",
                newName: "ProfileNotFound");
        }
    }
}
