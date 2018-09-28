using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class PlayerPastRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerPastRecord",
                columns: table => new
                {
                    PlayerPastRecordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                    TotalOvers = table.Column<int>(nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_PlayerPastRecord_PlayerId",
                table: "PlayerPastRecord",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerPastRecord");
        }
    }
}
