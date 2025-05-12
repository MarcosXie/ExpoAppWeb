using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpoApp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class fixgrouprelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_group_user_group_UserId",
                table: "group_user");

            migrationBuilder.CreateIndex(
                name: "IX_group_user_GroupId",
                table: "group_user",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_group_user_group_GroupId",
                table: "group_user",
                column: "GroupId",
                principalTable: "group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_group_user_group_GroupId",
                table: "group_user");

            migrationBuilder.DropIndex(
                name: "IX_group_user_GroupId",
                table: "group_user");

            migrationBuilder.AddForeignKey(
                name: "FK_group_user_group_UserId",
                table: "group_user",
                column: "UserId",
                principalTable: "group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
