using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expensi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFamilyMemberToRemitter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_FamilyMember_FamilyMemberId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "FamilyMember");

            migrationBuilder.RenameColumn(
                name: "FamilyMemberId",
                table: "Expenses",
                newName: "RemitterId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_FamilyMemberId",
                table: "Expenses",
                newName: "IX_Expenses_RemitterId");

            migrationBuilder.CreateTable(
                name: "Remitters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remitters", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Remitters_RemitterId",
                table: "Expenses",
                column: "RemitterId",
                principalTable: "Remitters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Remitters_RemitterId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "Remitters");

            migrationBuilder.RenameColumn(
                name: "RemitterId",
                table: "Expenses",
                newName: "FamilyMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_RemitterId",
                table: "Expenses",
                newName: "IX_Expenses_FamilyMemberId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_FamilyMember_FamilyMemberId",
                table: "Expenses",
                column: "FamilyMemberId",
                principalTable: "FamilyMember",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
