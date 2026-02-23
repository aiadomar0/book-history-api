using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHistory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DisableCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangeLogs_Books_BookId",
                table: "ChangeLogs");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookId",
                table: "ChangeLogs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeLogs_Books_BookId",
                table: "ChangeLogs",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangeLogs_Books_BookId",
                table: "ChangeLogs");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookId",
                table: "ChangeLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeLogs_Books_BookId",
                table: "ChangeLogs",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
