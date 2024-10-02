using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UExpo.Repository.Migrations
{
    /// <inheritdoc />
#pragma warning disable CS8981 // O nome do tipo contém apenas caracteres ascii em caixa baixa. Esses nomes podem ficar reservados para o idioma.
    public partial class modifyondeleteactioncatalogusers : Migration
#pragma warning restore CS8981 // O nome do tipo contém apenas caracteres ascii em caixa baixa. Esses nomes podem ficar reservados para o idioma.
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
