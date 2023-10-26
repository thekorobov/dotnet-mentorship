﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reminders.DAL.Migrations
{
    public partial class AddAuthProviderToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthProvider",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthProvider",
                table: "AspNetUsers");
        }
    }
}
