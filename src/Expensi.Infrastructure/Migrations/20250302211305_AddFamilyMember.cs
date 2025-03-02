using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expensi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFamilyMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FamilyMemberId",
                table: "Expenses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "FamilyMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMember", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_FamilyMemberId",
                table: "Expenses",
                column: "FamilyMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_FamilyMember_FamilyMemberId",
                table: "Expenses",
                column: "FamilyMemberId",
                principalTable: "FamilyMember",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_FamilyMember_FamilyMemberId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "FamilyMember");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_FamilyMemberId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "FamilyMemberId",
                table: "Expenses");
        }
    }
}
