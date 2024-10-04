using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UExpo.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addsresponsedmessageid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ResponsedMessageId",
                table: "relationship_message",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "ResponsedMessageId",
                table: "cart_message",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "ResponsedMessageId",
                table: "call_center_message",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponsedMessageId",
                table: "relationship_message");

            migrationBuilder.DropColumn(
                name: "ResponsedMessageId",
                table: "cart_message");

            migrationBuilder.DropColumn(
                name: "ResponsedMessageId",
                table: "call_center_message");
        }
    }
}
