using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProfileDataId",
                table: "Profiles",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ProfileData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SteamId = table.Column<long>(type: "INTEGER", nullable: true),
                    PersonaName = table.Column<string>(type: "TEXT", nullable: true),
                    RealName = table.Column<string>(type: "TEXT", nullable: true),
                    CommentAreaDisabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileData", x => x.Id);
                });

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

            migrationBuilder.DropTable(
                name: "ProfileData");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_ProfileDataId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ProfileDataId",
                table: "Profiles");
        }
    }
}
