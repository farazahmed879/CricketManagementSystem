using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class HOMEOPPOvers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "HomeTeamOvers",
                table: "Matches",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "OppTeamOvers",
                table: "Matches",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeTeamOvers",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "OppTeamOvers",
                table: "Matches");
        }
    }
}
