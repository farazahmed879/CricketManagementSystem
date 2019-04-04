using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class FileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerLogo",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Matches",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Matches");

            migrationBuilder.AddColumn<byte[]>(
                name: "PlayerLogo",
                table: "Players",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
