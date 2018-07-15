using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CricketApp.Data.Migrations
{
    public partial class PlayerRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayerRoleId",
                table: "Players",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayerRoleId",
                table: "Players",
                column: "PlayerRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_PlayerRole_PlayerRoleId",
                table: "Players",
                column: "PlayerRoleId",
                principalTable: "PlayerRole",
                principalColumn: "PlayerRoleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_PlayerRole_PlayerRoleId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_PlayerRoleId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerRoleId",
                table: "Players");
        }
    }
}
