using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class stagesT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TournamentStageId",
                table: "Matches",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentStageId",
                table: "Matches",
                column: "TournamentStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_TournamentStages_TournamentStageId",
                table: "Matches",
                column: "TournamentStageId",
                principalTable: "TournamentStages",
                principalColumn: "TournamentStageId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_TournamentStages_TournamentStageId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_TournamentStageId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TournamentStageId",
                table: "Matches");
        }
    }
}
