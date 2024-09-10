using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UExpo.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addsbuyerandsuplierlang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerLang",
                table: "relationship",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SupplierLang",
                table: "relationship",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerLang",
                table: "relationship");

            migrationBuilder.DropColumn(
                name: "SupplierLang",
                table: "relationship");
        }
    }
}
