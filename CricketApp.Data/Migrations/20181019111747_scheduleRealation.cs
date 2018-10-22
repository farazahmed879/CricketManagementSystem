using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class scheduleRealation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "MatchSchedule",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MatchSchedule_TeamId",
                table: "MatchSchedule",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchSchedule_Teams_TeamId",
                table: "MatchSchedule",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchSchedule_Teams_TeamId",
                table: "MatchSchedule");

            migrationBuilder.DropIndex(
                name: "IX_MatchSchedule_TeamId",
                table: "MatchSchedule");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "MatchSchedule");
        }
    }
}
