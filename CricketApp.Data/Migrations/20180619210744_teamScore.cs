using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CricketApp.Data.Migrations
{
    public partial class teamScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamScore",
                columns: table => new
                {
                    TeamScoreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Buys = table.Column<int>(nullable: false),
                    LegBuys = table.Column<int>(nullable: false),
                    MatchId = table.Column<int>(nullable: false),
                    NoBalls = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    TotalScore = table.Column<int>(nullable: false),
                    Wideballs = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamScore", x => x.TeamScoreId);
                    table.ForeignKey(
                        name: "FK_TeamScore_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamScore_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamScore_MatchId",
                table: "TeamScore",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamScore_TeamId",
                table: "TeamScore",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamScore");
        }
    }
}
