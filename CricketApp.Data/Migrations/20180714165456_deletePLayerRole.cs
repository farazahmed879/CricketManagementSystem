using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CricketApp.Data.Migrations
{
    public partial class deletePLayerRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_PlayerRole_PlayerRoleId1",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_PlayerRoleId1",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerRoleId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerRoleId1",
                table: "Players");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlayerRoleId",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerRoleId1",
                table: "Players",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayerRoleId1",
                table: "Players",
                column: "PlayerRoleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_PlayerRole_PlayerRoleId1",
                table: "Players",
                column: "PlayerRoleId1",
                principalTable: "PlayerRole",
                principalColumn: "PlayerRoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
