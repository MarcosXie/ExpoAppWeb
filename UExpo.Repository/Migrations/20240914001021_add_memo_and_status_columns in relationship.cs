using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UExpo.Repository.Migrations
{
    /// <inheritdoc />
#pragma warning disable IDE1006 // Estilos de Nomenclatura
    public partial class add_memo_and_status_columnsinrelationship : Migration
#pragma warning restore IDE1006 // Estilos de Nomenclatura
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyerMemo",
                table: "relationship",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "BuyerStatus",
                table: "relationship",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "SupplierMemo",
                table: "relationship",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "SupplierStatus",
                table: "relationship",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerMemo",
                table: "relationship");

            migrationBuilder.DropColumn(
                name: "BuyerStatus",
                table: "relationship");

            migrationBuilder.DropColumn(
                name: "SupplierMemo",
                table: "relationship");

            migrationBuilder.DropColumn(
                name: "SupplierStatus",
                table: "relationship");
        }
    }
}
