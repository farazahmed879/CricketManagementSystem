using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CricketApp.Data.Migrations
{
    public partial class changingInTeamScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LegBuys",
                table: "TeamScores",
                newName: "LegByes");

            migrationBuilder.RenameColumn(
                name: "Buys",
                table: "TeamScores",
                newName: "Byes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LegByes",
                table: "TeamScores",
                newName: "LegBuys");

            migrationBuilder.RenameColumn(
                name: "Byes",
                table: "TeamScores",
                newName: "Buys");
        }
    }
}
