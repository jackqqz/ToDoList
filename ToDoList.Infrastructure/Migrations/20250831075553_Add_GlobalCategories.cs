using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_GlobalCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "ToDoItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ToDoCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_CategoryId",
                table: "ToDoItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoCategories_Name",
                table: "ToDoCategories",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItems_ToDoCategories_CategoryId",
                table: "ToDoItems",
                column: "CategoryId",
                principalTable: "ToDoCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItems_ToDoCategories_CategoryId",
                table: "ToDoItems");

            migrationBuilder.DropTable(
                name: "ToDoCategories");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItems_CategoryId",
                table: "ToDoItems");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ToDoItems");
        }
    }
}
