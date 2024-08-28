using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UExpo.Repository.Migrations;

/// <inheritdoc />
public partial class addexhibitorfairregistertable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "exhibitor_fair_register",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                ExhibitorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                CalendarFairId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                Value = table.Column<double>(type: "double", nullable: false),
                IsPaid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("exhibitor_fair_register_pkey", x => x.Id);
                table.ForeignKey(
                    name: "FK_exhibitor_fair_register_calendar_fair_CalendarFairId",
                    column: x => x.CalendarFairId,
                    principalTable: "calendar_fair",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_exhibitor_fair_register_user_ExhibitorId",
                    column: x => x.ExhibitorId,
                    principalTable: "user",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateIndex(
            name: "IX_exhibitor_fair_register_CalendarFairId",
            table: "exhibitor_fair_register",
            column: "CalendarFairId");

        migrationBuilder.CreateIndex(
            name: "IX_exhibitor_fair_register_ExhibitorId",
            table: "exhibitor_fair_register",
            column: "ExhibitorId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "exhibitor_fair_register");
    }
}
