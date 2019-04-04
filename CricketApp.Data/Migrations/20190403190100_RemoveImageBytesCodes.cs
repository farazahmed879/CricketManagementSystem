using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class RemoveImageBytesCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamLogo",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "MatchLogo",
                table: "Matches");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "TeamLogo",
                table: "Teams",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "MatchLogo",
                table: "Matches",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
