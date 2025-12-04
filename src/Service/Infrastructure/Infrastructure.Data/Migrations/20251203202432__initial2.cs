using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class _initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_advertisements_users_UserId1",
                table: "advertisements");

            migrationBuilder.DropIndex(
                name: "IX_advertisements_UserId1",
                table: "advertisements");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "advertisements");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "advertisements",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_advertisements_UserId1",
                table: "advertisements",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_advertisements_users_UserId1",
                table: "advertisements",
                column: "UserId1",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
