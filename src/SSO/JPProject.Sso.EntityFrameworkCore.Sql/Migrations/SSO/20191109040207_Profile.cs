using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JPProject.Sso.EntityFrameworkCore.SqlServer.Migrations.SSO
{
    public partial class Profile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNumber",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SocialNumber",
                table: "Users");
        }
    }
}
