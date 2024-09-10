using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UExpo.Repository.Migrations
{
	/// <inheritdoc />
	public partial class modifytouserelationshipaschat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_relationship_message_relationship_chat_ChatId",
                table: "relationship_message");

            migrationBuilder.DropTable(
                name: "relationship_chat");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "relationship");

            migrationBuilder.AddForeignKey(
                name: "FK_relationship_message_relationship_ChatId",
                table: "relationship_message",
                column: "ChatId",
                principalTable: "relationship",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_relationship_message_relationship_ChatId",
                table: "relationship_message");

            migrationBuilder.AddColumn<Guid>(
                name: "ChatId",
                table: "relationship",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "relationship_chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BuyerUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SupplierUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("relationship_chat_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_relationship_chat_user_BuyerUserId",
                        column: x => x.BuyerUserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_relationship_chat_user_SupplierUserId",
                        column: x => x.SupplierUserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_relationship_chat_BuyerUserId",
                table: "relationship_chat",
                column: "BuyerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_relationship_chat_SupplierUserId",
                table: "relationship_chat",
                column: "SupplierUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_relationship_message_relationship_chat_ChatId",
                table: "relationship_message",
                column: "ChatId",
                principalTable: "relationship_chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
