using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class TournamentImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Tournaments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "MatchSeries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "MatchSeries");
        }
    }
}
