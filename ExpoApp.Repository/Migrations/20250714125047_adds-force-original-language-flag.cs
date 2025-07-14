using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpoApp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addsforceoriginallanguageflag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForceOriginalLanguage",
                table: "relationship_message",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ForceOriginalLanguage",
                table: "group_message",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ForceOriginalLanguage",
                table: "cart_message",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ForceOriginalLanguage",
                table: "call_center_message",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForceOriginalLanguage",
                table: "relationship_message");

            migrationBuilder.DropColumn(
                name: "ForceOriginalLanguage",
                table: "group_message");

            migrationBuilder.DropColumn(
                name: "ForceOriginalLanguage",
                table: "cart_message");

            migrationBuilder.DropColumn(
                name: "ForceOriginalLanguage",
                table: "call_center_message");
        }
    }
}
