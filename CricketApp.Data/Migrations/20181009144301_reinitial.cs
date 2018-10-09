using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class reinitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchSeriesId",
                table: "Matches",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MatchSeries",
                columns: table => new
                {
                    MatchSeriesId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Organizor = table.Column<string>(nullable: true),
                    StartingDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    TenantUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchSeries", x => x.MatchSeriesId);
                    table.ForeignKey(
                        name: "FK_MatchSeries_AspNetUsers_TenantUserId",
                        column: x => x.TenantUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_MatchSeriesId",
                table: "Matches",
                column: "MatchSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchSeries_TenantUserId",
                table: "MatchSeries",
                column: "TenantUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_MatchSeries_MatchSeriesId",
                table: "Matches",
                column: "MatchSeriesId",
                principalTable: "MatchSeries",
                principalColumn: "MatchSeriesId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_MatchSeries_MatchSeriesId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "MatchSeries");

            migrationBuilder.DropIndex(
                name: "IX_Matches_MatchSeriesId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchSeriesId",
                table: "Matches");
        }
    }
}
