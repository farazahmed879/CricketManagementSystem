using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CricketApp.Data.Migrations
{
    public partial class PLayerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HowOut",
                table: "PlayerScores");

            migrationBuilder.DropColumn(
                name: "BattingStyle",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "BowlingStyle",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Players",
                newName: "PlayerRoleId");

            migrationBuilder.AddColumn<int>(
                name: "HowOutId",
                table: "PlayerScores",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BattingStyleId",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BowlingStyleId",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerRoleId1",
                table: "Players",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BattingStyle",
                columns: table => new
                {
                    BattingStyleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattingStyle", x => x.BattingStyleId);
                });

            migrationBuilder.CreateTable(
                name: "BowlingStyle",
                columns: table => new
                {
                    BowlingStyleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BowlingStyle", x => x.BowlingStyleId);
                });

            migrationBuilder.CreateTable(
                name: "HowOut",
                columns: table => new
                {
                    HowOutId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HowOut", x => x.HowOutId);
                });

            migrationBuilder.CreateTable(
                name: "PlayerRole",
                columns: table => new
                {
                    PlayerRoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerRole", x => x.PlayerRoleId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerScores_HowOutId",
                table: "PlayerScores",
                column: "HowOutId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_BattingStyleId",
                table: "Players",
                column: "BattingStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_BowlingStyleId",
                table: "Players",
                column: "BowlingStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayerRoleId1",
                table: "Players",
                column: "PlayerRoleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_BattingStyle_BattingStyleId",
                table: "Players",
                column: "BattingStyleId",
                principalTable: "BattingStyle",
                principalColumn: "BattingStyleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_BowlingStyle_BowlingStyleId",
                table: "Players",
                column: "BowlingStyleId",
                principalTable: "BowlingStyle",
                principalColumn: "BowlingStyleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_PlayerRole_PlayerRoleId1",
                table: "Players",
                column: "PlayerRoleId1",
                principalTable: "PlayerRole",
                principalColumn: "PlayerRoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerScores_HowOut_HowOutId",
                table: "PlayerScores",
                column: "HowOutId",
                principalTable: "HowOut",
                principalColumn: "HowOutId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_BattingStyle_BattingStyleId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_BowlingStyle_BowlingStyleId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_PlayerRole_PlayerRoleId1",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerScores_HowOut_HowOutId",
                table: "PlayerScores");

            migrationBuilder.DropTable(
                name: "BattingStyle");

            migrationBuilder.DropTable(
                name: "BowlingStyle");

            migrationBuilder.DropTable(
                name: "HowOut");

            migrationBuilder.DropTable(
                name: "PlayerRole");

            migrationBuilder.DropIndex(
                name: "IX_PlayerScores_HowOutId",
                table: "PlayerScores");

            migrationBuilder.DropIndex(
                name: "IX_Players_BattingStyleId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_BowlingStyleId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_PlayerRoleId1",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "HowOutId",
                table: "PlayerScores");

            migrationBuilder.DropColumn(
                name: "BattingStyleId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "BowlingStyleId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerRoleId1",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "PlayerRoleId",
                table: "Players",
                newName: "Role");

            migrationBuilder.AddColumn<string>(
                name: "HowOut",
                table: "PlayerScores",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BattingStyle",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BowlingStyle",
                table: "Players",
                nullable: true);
        }
    }
}
