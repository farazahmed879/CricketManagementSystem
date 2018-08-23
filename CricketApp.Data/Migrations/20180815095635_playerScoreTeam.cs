using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CricketApp.Data.Migrations
{
    public partial class playerScoreTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "PlayerScores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerScores_TeamId",
                table: "PlayerScores",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerScores_Teams_TeamId",
                table: "PlayerScores",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerScores_Teams_TeamId",
                table: "PlayerScores");

            migrationBuilder.DropIndex(
                name: "IX_PlayerScores_TeamId",
                table: "PlayerScores");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "PlayerScores");
        }
    }
}
