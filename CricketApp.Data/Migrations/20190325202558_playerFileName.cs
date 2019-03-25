using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class playerFileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Players",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Players");
        }
    }
}
