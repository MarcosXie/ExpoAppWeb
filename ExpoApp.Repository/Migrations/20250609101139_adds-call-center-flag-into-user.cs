using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpoApp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addscallcenterflagintouser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCallCenter",
                table: "user",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCallCenter",
                table: "user");
        }
    }
}
