using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reminders.DAL.Migrations
{
    public partial class RenameAuthProvider2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthProviderFilter",
                table: "AspNetUsers",
                newName: "AuthProviderType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthProviderType",
                table: "AspNetUsers",
                newName: "AuthProviderFilter");
        }
    }
}
