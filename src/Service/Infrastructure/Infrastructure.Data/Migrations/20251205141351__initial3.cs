using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class _initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "rating",
                table: "advertisements",
                type: "integer",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "advertisements",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "rating",
                table: "advertisements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "advertisements",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");
        }
    }
}
