using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreakyAPi.Migrations
{
    /// <inheritdoc />
    public partial class fixlistcategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Categories_CategoryId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CategoryId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCategories", x => new { x.CategoryId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCategories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCategories_UserId",
                table: "UserCategories",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCategories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CategoryId",
                table: "Users",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Categories_CategoryId",
                table: "Users",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
