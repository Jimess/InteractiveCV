using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InteractiveCV.Migrations
{
    public partial class DBV4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Example",
                table: "Skills");

            migrationBuilder.AddColumn<string>(
                name: "RelatedExperience",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelatedExperience",
                table: "Skills");

            migrationBuilder.AddColumn<string>(
                name: "Example",
                table: "Skills",
                nullable: true);
        }
    }
}
