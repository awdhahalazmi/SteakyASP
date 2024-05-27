using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreakyAPi.Migrations
{
    /// <inheritdoc />
    public partial class Changinglocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Businesses_BusinessId",
                table: "Locations");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessId",
                table: "Locations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Businesses_BusinessId",
                table: "Locations",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Businesses_BusinessId",
                table: "Locations");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessId",
                table: "Locations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Businesses_BusinessId",
                table: "Locations",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
