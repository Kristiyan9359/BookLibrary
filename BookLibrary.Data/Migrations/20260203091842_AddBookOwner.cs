using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBookOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Books",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Books_OwnerId",
                table: "Books",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_OwnerId",
                table: "Books",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_OwnerId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_OwnerId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Books");
        }
    }
}
