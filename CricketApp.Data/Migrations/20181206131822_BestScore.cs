using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class BestScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BestScore",
                table: "PlayerPastRecord",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BestScore",
                table: "PlayerPastRecord");
        }
    }
}
