using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    /// <inheritdoc />
    public partial class Profile_ProfileNotFound : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ProfileNotFound",
                table: "Profiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileNotFound",
                table: "Profiles");
        }
    }
}
