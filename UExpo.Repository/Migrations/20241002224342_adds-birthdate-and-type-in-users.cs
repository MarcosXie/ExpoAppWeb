using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UExpo.Repository.Migrations
{
	/// <inheritdoc />
#pragma warning disable CS8981 // O nome do tipo contém apenas caracteres ascii em caixa baixa. Esses nomes podem ficar reservados para o idioma.
	public partial class addsbirthdateandtypeinusers : Migration
#pragma warning restore CS8981 // O nome do tipo contém apenas caracteres ascii em caixa baixa. Esses nomes podem ficar reservados para o idioma.
	{
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "user",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "user",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "user");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "user");
        }
    }
}
