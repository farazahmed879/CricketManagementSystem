using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketApp.Data.Migrations
{
    public partial class RemoveClubAdminAddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubAdmins");

            migrationBuilder.AddColumn<int>(
                name: "TenantUserId",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserRoleId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TenantUserId",
                table: "Teams",
                column: "TenantUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ApplicationUserRoleId",
                table: "AspNetUsers",
                column: "ApplicationUserRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_ApplicationUserRoleId",
                table: "AspNetUsers",
                column: "ApplicationUserRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_TenantUserId",
                table: "Teams",
                column: "TenantUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_ApplicationUserRoleId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_TenantUserId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_TenantUserId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ApplicationUserRoleId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TenantUserId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ApplicationUserRoleId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ClubAdmins",
                columns: table => new
                {
                    ClubAdminId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeamId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubAdmins", x => x.ClubAdminId);
                    table.ForeignKey(
                        name: "FK_ClubAdmins_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubAdmins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdmins_TeamId",
                table: "ClubAdmins",
                column: "TeamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdmins_UserId",
                table: "ClubAdmins",
                column: "UserId");
        }
    }
}
