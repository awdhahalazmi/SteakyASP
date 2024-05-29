﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreakyAPi.Migrations
{
    /// <inheritdoc />
    public partial class fixedTheBusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Businesses_BusinessId",
                table: "Locations");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Businesses_BusinessId",
                table: "Locations",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Businesses_BusinessId",
                table: "Locations");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Businesses_BusinessId",
                table: "Locations",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id");
        }
    }
}
