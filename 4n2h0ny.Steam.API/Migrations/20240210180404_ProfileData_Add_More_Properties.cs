using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    /// <inheritdoc />
    public partial class ProfileData_Add_More_Properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AwardCount",
                table: "ProfileData",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BadgeCount",
                table: "ProfileData",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommonFriendCount",
                table: "ProfileData",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FriendCount",
                table: "ProfileData",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "ProfileData",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwardCount",
                table: "ProfileData");

            migrationBuilder.DropColumn(
                name: "BadgeCount",
                table: "ProfileData");

            migrationBuilder.DropColumn(
                name: "CommonFriendCount",
                table: "ProfileData");

            migrationBuilder.DropColumn(
                name: "FriendCount",
                table: "ProfileData");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "ProfileData");
        }
    }
}
