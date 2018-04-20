using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CricketApp.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    contact = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(nullable: true),
                    Place = table.Column<string>(nullable: true),
                    TeamLogo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Team_Name = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    TournamentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Organizor = table.Column<string>(nullable: true),
                    TournamentName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.TournamentId);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    BattingStyle = table.Column<string>(nullable: true),
                    BowlingStyle = table.Column<string>(nullable: true),
                    CNIC = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    IsGuestPlayer = table.Column<bool>(nullable: false),
                    MatchId = table.Column<int>(nullable: false),
                    PlayerLogo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Player_Name = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    TeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    MatchId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateOfMatch = table.Column<DateTime>(nullable: true),
                    GroundName = table.Column<string>(nullable: true),
                    HomeTeamId = table.Column<int>(nullable: false),
                    MatchLogo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    MatchOvers = table.Column<int>(nullable: false),
                    OppponentTeamId = table.Column<int>(nullable: false),
                    Place = table.Column<string>(nullable: true),
                    Result = table.Column<string>(nullable: true),
                    Season = table.Column<int>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    TournamentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.MatchId);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_OppponentTeamId",
                        column: x => x.OppponentTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerScores",
                columns: table => new
                {
                    PlayerScoreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ball_Runs = table.Column<int>(nullable: true),
                    Bat_Balls = table.Column<int>(nullable: true),
                    Bat_Runs = table.Column<int>(nullable: true),
                    Bowler = table.Column<string>(nullable: true),
                    Catches = table.Column<int>(nullable: true),
                    Four = table.Column<int>(nullable: true),
                    HowOut = table.Column<string>(nullable: true),
                    IsPlayedInning = table.Column<bool>(nullable: false),
                    Maiden = table.Column<int>(nullable: true),
                    MatchId = table.Column<int>(nullable: false),
                    Overs = table.Column<int>(nullable: true),
                    PlayerId = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    RunOut = table.Column<int>(nullable: true),
                    Six = table.Column<int>(nullable: true),
                    Stump = table.Column<int>(nullable: true),
                    Wickets = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerScores", x => x.PlayerScoreId);
                    table.ForeignKey(
                        name: "FK_PlayerScores_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerScores_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamId",
                table: "Matches",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_OppponentTeamId",
                table: "Matches",
                column: "OppponentTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentId",
                table: "Matches",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerScores_MatchId",
                table: "PlayerScores",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerScores_PlayerId",
                table: "PlayerScores",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "PlayerScores");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
