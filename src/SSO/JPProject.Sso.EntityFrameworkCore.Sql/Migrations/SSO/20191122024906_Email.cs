using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JPProject.Sso.EntityFrameworkCore.SqlServer.Migrations.SSO
{
    public partial class Email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(maxLength: 250, nullable: false),
                    Sender_Address = table.Column<string>(maxLength: 250, nullable: true),
                    Sender_Name = table.Column<string>(maxLength: 250, nullable: true),
                    Bcc__recipientsCollection = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    Content = table.Column<string>(maxLength: 2147483647, nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "Templates");
        }
    }
}
