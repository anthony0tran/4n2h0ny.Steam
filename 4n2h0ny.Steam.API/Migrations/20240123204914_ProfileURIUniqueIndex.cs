using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    /// <inheritdoc />
    public partial class ProfileURIUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileUrl",
                table: "Profiles",
                newName: "URI");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_URI",
                table: "Profiles",
                column: "URI",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_URI",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "URI",
                table: "Profiles",
                newName: "ProfileUrl");
        }
    }
}
