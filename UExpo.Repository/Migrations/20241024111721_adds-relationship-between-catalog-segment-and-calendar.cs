using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UExpo.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addsrelationshipbetweencatalogsegmentandcalendar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CalendarId",
                table: "catalog_segment",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_catalog_segment_CalendarId",
                table: "catalog_segment",
                column: "CalendarId");

            migrationBuilder.AddForeignKey(
                name: "FK_catalog_segment_calendar_CalendarId",
                table: "catalog_segment",
                column: "CalendarId",
                principalTable: "calendar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_catalog_segment_calendar_CalendarId",
                table: "catalog_segment");

            migrationBuilder.DropIndex(
                name: "IX_catalog_segment_CalendarId",
                table: "catalog_segment");

            migrationBuilder.DropColumn(
                name: "CalendarId",
                table: "catalog_segment");
        }
    }
}
