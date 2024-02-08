using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileDataForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Profiles_ProfileDataId",
                table: "Profiles",
                column: "ProfileDataId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_ProfileData_ProfileDataId",
                table: "Profiles",
                column: "ProfileDataId",
                principalTable: "ProfileData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
            name: "FK_Profiles_ProfileData_ProfileDataId",
            table: "Profiles");

            migrationBuilder.DropIndex(
            name: "IX_Profiles_ProfileDataId",
            table: "Profiles");
        }
    }
}
