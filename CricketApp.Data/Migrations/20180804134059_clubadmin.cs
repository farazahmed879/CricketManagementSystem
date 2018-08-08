using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CricketApp.Data.Migrations
{
    public partial class clubadmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_ClubUserId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_ClubUserId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ClubUserId",
                table: "Teams");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubUserId",
                table: "Teams",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ClubUserId",
                table: "Teams",
                column: "ClubUserId",
                unique: true,
                filter: "ClubUserId is not null");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_ClubUserId",
                table: "Teams",
                column: "ClubUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
