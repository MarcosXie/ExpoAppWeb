using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UExpo.Repository.Migrations
{
    /// <inheritdoc />
#pragma warning disable CS8981 // O nome do tipo contém apenas caracteres ascii em caixa baixa. Esses nomes podem ficar reservados para o idioma.
#pragma warning disable IDE1006 // Estilos de Nomenclatura
    public partial class addsimagecolumnincartitem : Migration
#pragma warning restore IDE1006 // Estilos de Nomenclatura
#pragma warning restore CS8981 // O nome do tipo contém apenas caracteres ascii em caixa baixa. Esses nomes podem ficar reservados para o idioma.
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "cart_item",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "cart_item");
        }
    }
}
