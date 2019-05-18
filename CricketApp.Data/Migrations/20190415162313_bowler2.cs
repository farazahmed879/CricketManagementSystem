using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class bowler2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bowler",
                table: "PlayerScores");

            migrationBuilder.AddColumn<int>(
                name: "BowlerId",
                table: "PlayerScores",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerScores_BowlerId",
                table: "PlayerScores",
                column: "BowlerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerScores_Players_BowlerId",
                table: "PlayerScores",
                column: "BowlerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerScores_Players_BowlerId",
                table: "PlayerScores");

            migrationBuilder.DropIndex(
                name: "IX_PlayerScores_BowlerId",
                table: "PlayerScores");

            migrationBuilder.DropColumn(
                name: "BowlerId",
                table: "PlayerScores");

            migrationBuilder.AddColumn<string>(
                name: "Bowler",
                table: "PlayerScores",
                nullable: true);
        }
    }
}
