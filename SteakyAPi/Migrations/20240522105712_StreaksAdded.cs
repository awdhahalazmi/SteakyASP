using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreakyAPi.Migrations
{
    /// <inheritdoc />
    public partial class StreaksAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StreakDate",
                table: "UserStreaks",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "UserStreaks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "StreakId",
                table: "UserStreaks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Streaks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streaks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StreakBusiness",
                columns: table => new
                {
                    BusinessId = table.Column<int>(type: "int", nullable: false),
                    StreakId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreakBusiness", x => new { x.BusinessId, x.StreakId });
                    table.ForeignKey(
                        name: "FK_StreakBusiness_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreakBusiness_Streaks_StreakId",
                        column: x => x.StreakId,
                        principalTable: "Streaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStreaks_StreakId",
                table: "UserStreaks",
                column: "StreakId");

            migrationBuilder.CreateIndex(
                name: "IX_StreakBusiness_StreakId",
                table: "StreakBusiness",
                column: "StreakId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStreaks_Streaks_StreakId",
                table: "UserStreaks",
                column: "StreakId",
                principalTable: "Streaks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStreaks_Streaks_StreakId",
                table: "UserStreaks");

            migrationBuilder.DropTable(
                name: "StreakBusiness");

            migrationBuilder.DropTable(
                name: "Streaks");

            migrationBuilder.DropIndex(
                name: "IX_UserStreaks_StreakId",
                table: "UserStreaks");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "UserStreaks");

            migrationBuilder.DropColumn(
                name: "StreakId",
                table: "UserStreaks");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "UserStreaks",
                newName: "StreakDate");
        }
    }
}
