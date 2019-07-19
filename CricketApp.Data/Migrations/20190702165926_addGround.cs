using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class addGround : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroundName",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Place",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "GroundId",
                table: "Matches",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Ground",
                columns: table => new
                {
                    GroundId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 25, nullable: true),
                    Location = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ground", x => x.GroundId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_GroundId",
                table: "Matches",
                column: "GroundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Ground_GroundId",
                table: "Matches",
                column: "GroundId",
                principalTable: "Ground",
                principalColumn: "GroundId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Ground_GroundId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "Ground");

            migrationBuilder.DropIndex(
                name: "IX_Matches_GroundId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "GroundId",
                table: "Matches");

            migrationBuilder.AddColumn<string>(
                name: "GroundName",
                table: "Matches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Place",
                table: "Matches",
                nullable: true);
        }
    }
}
