using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UExpo.Repository.Migrations
{
    /// <inheritdoc />
    public partial class modifyondeleteactioncatalogusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_catalog_user_UserId",
                table: "catalog");

            migrationBuilder.AddForeignKey(
                name: "FK_catalog_user_UserId",
                table: "catalog",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_catalog_user_UserId",
                table: "catalog");

            migrationBuilder.AddForeignKey(
                name: "FK_catalog_user_UserId",
                table: "catalog",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id");
        }
    }
}
