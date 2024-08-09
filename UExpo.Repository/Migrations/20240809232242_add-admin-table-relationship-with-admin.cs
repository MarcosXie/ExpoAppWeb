using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UExpo.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addadmintablerelationshipwithadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_call_center_chat_Admins_AdminId",
                table: "call_center_chat");

            migrationBuilder.AddForeignKey(
                name: "FK_call_center_chat_Admins_AdminId",
                table: "call_center_chat",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_call_center_chat_Admins_AdminId",
                table: "call_center_chat");

            migrationBuilder.AddForeignKey(
                name: "FK_call_center_chat_Admins_AdminId",
                table: "call_center_chat",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");
        }
    }
}
