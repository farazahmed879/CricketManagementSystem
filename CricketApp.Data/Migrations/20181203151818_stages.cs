using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class stages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGuestPlayer",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "IsGuestorRegistered",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchDescription",
                table: "Matches",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TossWinningTeam",
                table: "Matches",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TournamentStages",
                columns: table => new
                {
                    TournamentStageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentStages", x => x.TournamentStageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TournamentStages");

            migrationBuilder.DropColumn(
                name: "IsGuestorRegistered",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "MatchDescription",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TossWinningTeam",
                table: "Matches");

            migrationBuilder.AddColumn<bool>(
                name: "IsGuestPlayer",
                table: "Players",
                nullable: false,
                defaultValue: false);
        }
    }
}
