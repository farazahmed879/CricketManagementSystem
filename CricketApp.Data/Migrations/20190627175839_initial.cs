using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BattingStyle",
                columns: table => new
                {
                    BattingStyleId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattingStyle", x => x.BattingStyleId);
                });

            migrationBuilder.CreateTable(
                name: "BowlingStyle",
                columns: table => new
                {
                    BowlingStyleId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BowlingStyle", x => x.BowlingStyleId);
                });

            migrationBuilder.CreateTable(
                name: "HowOut",
                columns: table => new
                {
                    HowOutId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Normalize = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HowOut", x => x.HowOutId);
                });

            migrationBuilder.CreateTable(
                name: "MatchType",
                columns: table => new
                {
                    MatchTypeId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MatchTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchType", x => x.MatchTypeId);
                });

            migrationBuilder.CreateTable(
                name: "PlayerRole",
                columns: table => new
                {
                    PlayerRoleId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerRole", x => x.PlayerRoleId);
                });

            migrationBuilder.CreateTable(
                name: "TournamentStages",
                columns: table => new
                {
                    TournamentStageId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentStages", x => x.TournamentStageId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    ApplicationUserRoleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetRoles_ApplicationUserRoleId",
                        column: x => x.ApplicationUserRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    UserId1 = table.Column<int>(nullable: true),
                    RoleId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId1",
                        column: x => x.RoleId1,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchSeries",
                columns: table => new
                {
                    MatchSeriesId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Organizor = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    StartingDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    TenantUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchSeries", x => x.MatchSeriesId);
                    table.ForeignKey(
                        name: "FK_MatchSeries_AspNetUsers_TenantUserId",
                        column: x => x.TenantUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Team_Name = table.Column<string>(nullable: false),
                    Place = table.Column<string>(maxLength: 100, nullable: true),
                    Zone = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    IsRegistered = table.Column<bool>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Teams_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    TournamentId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TournamentName = table.Column<string>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    Organizor = table.Column<string>(nullable: true),
                    StartingDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    TenantUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.TournamentId);
                    table.ForeignKey(
                        name: "FK_Tournaments_AspNetUsers_TenantUserId",
                        column: x => x.TenantUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatchSchedule",
                columns: table => new
                {
                    MatchScheduleId = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroundName = table.Column<string>(nullable: true),
                    OpponentTeam = table.Column<string>(nullable: true),
                    Month = table.Column<string>(nullable: true),
                    Day = table.Column<int>(nullable: true),
                    Year = table.Column<int>(nullable: true),
                    TeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchSchedule", x => x.MatchScheduleId);
                    table.ForeignKey(
                        name: "FK_MatchSchedule_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Player_Name = table.Column<string>(nullable: false),
                    Contact = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    CNIC = table.Column<string>(nullable: true),
                    BattingStyleId = table.Column<int>(nullable: true),
                    BowlingStyleId = table.Column<int>(nullable: true),
                    PlayerRoleId = table.Column<int>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    IsGuestorRegistered = table.Column<string>(nullable: true),
                    IsDeactivated = table.Column<bool>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Players_BattingStyle_BattingStyleId",
                        column: x => x.BattingStyleId,
                        principalTable: "BattingStyle",
                        principalColumn: "BattingStyleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Players_BowlingStyle_BowlingStyleId",
                        column: x => x.BowlingStyleId,
                        principalTable: "BowlingStyle",
                        principalColumn: "BowlingStyleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Players_PlayerRole_PlayerRoleId",
                        column: x => x.PlayerRoleId,
                        principalTable: "PlayerRole",
                        principalColumn: "PlayerRoleId",
                        onDelete: ReferentialAction.Restrict);
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
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroundName = table.Column<string>(nullable: true),
                    Place = table.Column<string>(nullable: true),
                    MatchOvers = table.Column<int>(nullable: false),
                    Result = table.Column<string>(nullable: false),
                    MatchDescription = table.Column<string>(nullable: true),
                    Season = table.Column<int>(nullable: true),
                    TournamentId = table.Column<int>(nullable: true),
                    TossWinningTeam = table.Column<int>(nullable: true),
                    DateOfMatch = table.Column<DateTime>(nullable: true),
                    HomeTeamId = table.Column<int>(nullable: false),
                    OppponentTeamId = table.Column<int>(nullable: false),
                    HomeTeamOvers = table.Column<float>(nullable: true),
                    OppTeamOvers = table.Column<float>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    MatchTypeId = table.Column<int>(nullable: false),
                    TournamentStageId = table.Column<int>(nullable: true),
                    MatchSeriesId = table.Column<int>(nullable: true),
                    PlayerOTM = table.Column<int>(nullable: true),
                    PlayerId = table.Column<int>(nullable: true)
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
                        name: "FK_Matches_MatchSeries_MatchSeriesId",
                        column: x => x.MatchSeriesId,
                        principalTable: "MatchSeries",
                        principalColumn: "MatchSeriesId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_MatchType_MatchTypeId",
                        column: x => x.MatchTypeId,
                        principalTable: "MatchType",
                        principalColumn: "MatchTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_OppponentTeamId",
                        column: x => x.OppponentTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_TournamentStages_TournamentStageId",
                        column: x => x.TournamentStageId,
                        principalTable: "TournamentStages",
                        principalColumn: "TournamentStageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerPastRecord",
                columns: table => new
                {
                    PlayerPastRecordId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TotalMatch = table.Column<int>(nullable: true),
                    TotalInnings = table.Column<int>(nullable: true),
                    TotalNotOut = table.Column<int>(nullable: true),
                    GetBowled = table.Column<int>(nullable: true),
                    GetHitWicket = table.Column<int>(nullable: true),
                    GetLBW = table.Column<int>(nullable: true),
                    GetCatch = table.Column<int>(nullable: true),
                    GetStump = table.Column<int>(nullable: true),
                    GetRunOut = table.Column<int>(nullable: true),
                    TotalBatRuns = table.Column<int>(nullable: true),
                    TotalBatBalls = table.Column<int>(nullable: true),
                    TotalFours = table.Column<int>(nullable: true),
                    TotalSixes = table.Column<int>(nullable: true),
                    NumberOf50s = table.Column<int>(nullable: true),
                    NumberOf100s = table.Column<int>(nullable: true),
                    BestScore = table.Column<int>(nullable: true),
                    TotalOvers = table.Column<float>(nullable: true),
                    TotalBallRuns = table.Column<int>(nullable: true),
                    TotalWickets = table.Column<int>(nullable: true),
                    TotalMaidens = table.Column<int>(nullable: true),
                    FiveWickets = table.Column<int>(nullable: true),
                    DoBowled = table.Column<int>(nullable: true),
                    DoHitWicket = table.Column<int>(nullable: true),
                    DoLBW = table.Column<int>(nullable: true),
                    DoCatch = table.Column<int>(nullable: true),
                    DoStump = table.Column<int>(nullable: true),
                    OnFieldCatch = table.Column<int>(nullable: true),
                    OnFieldStump = table.Column<int>(nullable: true),
                    OnFieldRunOut = table.Column<int>(nullable: true),
                    PlayerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerPastRecord", x => x.PlayerPastRecordId);
                    table.ForeignKey(
                        name: "FK_PlayerPastRecord_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FallOFWickets",
                columns: table => new
                {
                    FallOfWicketId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MatchId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    First = table.Column<int>(nullable: false),
                    Second = table.Column<int>(nullable: false),
                    Third = table.Column<int>(nullable: false),
                    Fourth = table.Column<int>(nullable: false),
                    Fifth = table.Column<int>(nullable: false),
                    Sixth = table.Column<int>(nullable: false),
                    Seventh = table.Column<int>(nullable: false),
                    Eight = table.Column<int>(nullable: false),
                    Ninth = table.Column<int>(nullable: false),
                    Tenth = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FallOFWickets", x => x.FallOfWicketId);
                    table.ForeignKey(
                        name: "FK_FallOFWickets_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FallOFWickets_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerScores",
                columns: table => new
                {
                    PlayerScoreId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<int>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    MatchId = table.Column<int>(nullable: false),
                    BowlerId = table.Column<int>(nullable: true),
                    Bat_Runs = table.Column<int>(nullable: true),
                    Bat_Balls = table.Column<int>(nullable: true),
                    HowOutId = table.Column<int>(nullable: true),
                    Ball_Runs = table.Column<int>(nullable: true),
                    Overs = table.Column<float>(nullable: true),
                    Wickets = table.Column<int>(nullable: true),
                    Stump = table.Column<int>(nullable: true),
                    Catches = table.Column<int>(nullable: true),
                    Maiden = table.Column<int>(nullable: true),
                    RunOut = table.Column<int>(nullable: true),
                    Four = table.Column<int>(nullable: true),
                    Six = table.Column<int>(nullable: true),
                    Fielder = table.Column<string>(nullable: true),
                    TeamId = table.Column<int>(nullable: false),
                    IsPlayedInning = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerScores", x => x.PlayerScoreId);
                    table.ForeignKey(
                        name: "FK_PlayerScores_Players_BowlerId",
                        column: x => x.BowlerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerScores_HowOut_HowOutId",
                        column: x => x.HowOutId,
                        principalTable: "HowOut",
                        principalColumn: "HowOutId",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerScores_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamScores",
                columns: table => new
                {
                    TeamScoreId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TotalScore = table.Column<int>(nullable: false),
                    Wickets = table.Column<int>(nullable: false),
                    Wideballs = table.Column<int>(nullable: false),
                    NoBalls = table.Column<int>(nullable: false),
                    Byes = table.Column<int>(nullable: false),
                    LegByes = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    MatchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamScores", x => x.TeamScoreId);
                    table.ForeignKey(
                        name: "FK_TeamScores_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamScores_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId1",
                table: "AspNetUserRoles",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId1",
                table: "AspNetUserRoles",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ApplicationUserRoleId",
                table: "AspNetUsers",
                column: "ApplicationUserRoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FallOFWickets_MatchId",
                table: "FallOFWickets",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_FallOFWickets_TeamId",
                table: "FallOFWickets",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamId",
                table: "Matches",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_MatchSeriesId",
                table: "Matches",
                column: "MatchSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_MatchTypeId",
                table: "Matches",
                column: "MatchTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_OppponentTeamId",
                table: "Matches",
                column: "OppponentTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_PlayerId",
                table: "Matches",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentId",
                table: "Matches",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentStageId",
                table: "Matches",
                column: "TournamentStageId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchSchedule_TeamId",
                table: "MatchSchedule",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchSeries_TenantUserId",
                table: "MatchSeries",
                column: "TenantUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerPastRecord_PlayerId",
                table: "PlayerPastRecord",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_BattingStyleId",
                table: "Players",
                column: "BattingStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_BowlingStyleId",
                table: "Players",
                column: "BowlingStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayerRoleId",
                table: "Players",
                column: "PlayerRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerScores_BowlerId",
                table: "PlayerScores",
                column: "BowlerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerScores_HowOutId",
                table: "PlayerScores",
                column: "HowOutId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerScores_MatchId",
                table: "PlayerScores",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerScores_PlayerId",
                table: "PlayerScores",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerScores_TeamId",
                table: "PlayerScores",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_UserId",
                table: "Teams",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamScores_MatchId",
                table: "TeamScores",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamScores_TeamId",
                table: "TeamScores",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_TenantUserId",
                table: "Tournaments",
                column: "TenantUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "FallOFWickets");

            migrationBuilder.DropTable(
                name: "MatchSchedule");

            migrationBuilder.DropTable(
                name: "PlayerPastRecord");

            migrationBuilder.DropTable(
                name: "PlayerScores");

            migrationBuilder.DropTable(
                name: "TeamScores");

            migrationBuilder.DropTable(
                name: "HowOut");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "MatchSeries");

            migrationBuilder.DropTable(
                name: "MatchType");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "TournamentStages");

            migrationBuilder.DropTable(
                name: "BattingStyle");

            migrationBuilder.DropTable(
                name: "BowlingStyle");

            migrationBuilder.DropTable(
                name: "PlayerRole");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");
        }
    }
}
