using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpoApp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addstargetusertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TargetUserId",
                table: "momento",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_momento_TargetUserId",
                table: "momento",
                column: "TargetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_momento_user_TargetUserId",
                table: "momento",
                column: "TargetUserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_momento_user_TargetUserId",
                table: "momento");

            migrationBuilder.DropIndex(
                name: "IX_momento_TargetUserId",
                table: "momento");

            migrationBuilder.DropColumn(
                name: "TargetUserId",
                table: "momento");
        }
    }
}
