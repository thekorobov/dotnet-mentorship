using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reminders.DAL.Migrations
{
    public partial class RenameAuthProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthProvider",
                table: "AspNetUsers",
                newName: "AuthProviderFilter");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthProviderFilter",
                table: "AspNetUsers",
                newName: "AuthProvider");
        }
    }
}
