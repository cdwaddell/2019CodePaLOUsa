using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroServiceDemo.Api.Auth.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Bio = table.Column<string>(nullable: true),
                    Image = table.Column<string>(maxLength: 1024, nullable: true),
                    Email = table.Column<string>(maxLength: 1024, nullable: false),
                    Username = table.Column<string>(maxLength: 128, nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 1024, nullable: true),
                    Salt = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_Email", x => x.Email);
                    table.UniqueConstraint("AK_Users_Username", x => x.Username);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "auth");
        }
    }
}
