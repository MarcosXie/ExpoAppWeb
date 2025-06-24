using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpoApp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addsrelationshipnicknames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerEnterpriseNickName",
                table: "relationship",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BuyerNickName",
                table: "relationship",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SupplierEnterpriseNickName",
                table: "relationship",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SupplierNickName",
                table: "relationship",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerEnterpriseNickName",
                table: "relationship");

            migrationBuilder.DropColumn(
                name: "BuyerNickName",
                table: "relationship");

            migrationBuilder.DropColumn(
                name: "SupplierEnterpriseNickName",
                table: "relationship");

            migrationBuilder.DropColumn(
                name: "SupplierNickName",
                table: "relationship");
        }
    }
}
