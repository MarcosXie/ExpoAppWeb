using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpoApp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addsgroupimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImageName",
                table: "group",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUri",
                table: "group",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageName",
                table: "group");

            migrationBuilder.DropColumn(
                name: "ProfileImageUri",
                table: "group");
        }
    }
}
