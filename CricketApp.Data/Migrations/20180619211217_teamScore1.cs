using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CricketApp.Data.Migrations
{
    public partial class teamScore1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamScore_Matches_MatchId",
                table: "TeamScore");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamScore_Teams_TeamId",
                table: "TeamScore");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamScore",
                table: "TeamScore");

            migrationBuilder.RenameTable(
                name: "TeamScore",
                newName: "TeamScores");

            migrationBuilder.RenameIndex(
                name: "IX_TeamScore_TeamId",
                table: "TeamScores",
                newName: "IX_TeamScores_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamScore_MatchId",
                table: "TeamScores",
                newName: "IX_TeamScores_MatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamScores",
                table: "TeamScores",
                column: "TeamScoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamScores_Matches_MatchId",
                table: "TeamScores",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamScores_Teams_TeamId",
                table: "TeamScores",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamScores_Matches_MatchId",
                table: "TeamScores");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamScores_Teams_TeamId",
                table: "TeamScores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamScores",
                table: "TeamScores");

            migrationBuilder.RenameTable(
                name: "TeamScores",
                newName: "TeamScore");

            migrationBuilder.RenameIndex(
                name: "IX_TeamScores_TeamId",
                table: "TeamScore",
                newName: "IX_TeamScore_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamScores_MatchId",
                table: "TeamScore",
                newName: "IX_TeamScore_MatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamScore",
                table: "TeamScore",
                column: "TeamScoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamScore_Matches_MatchId",
                table: "TeamScore",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "MatchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamScore_Teams_TeamId",
                table: "TeamScore",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
