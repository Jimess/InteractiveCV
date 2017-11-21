using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InteractiveCV.Migrations
{
    public partial class DBV62 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillLink_Skills_SkillsID",
                table: "SkillLink");

            migrationBuilder.RenameColumn(
                name: "SkillsID",
                table: "SkillLink",
                newName: "skillsID");

            migrationBuilder.RenameIndex(
                name: "IX_SkillLink_SkillsID",
                table: "SkillLink",
                newName: "IX_SkillLink_skillsID");

            migrationBuilder.AlterColumn<int>(
                name: "skillsID",
                table: "SkillLink",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLink_Skills_skillsID",
                table: "SkillLink",
                column: "skillsID",
                principalTable: "Skills",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillLink_Skills_skillsID",
                table: "SkillLink");

            migrationBuilder.RenameColumn(
                name: "skillsID",
                table: "SkillLink",
                newName: "SkillsID");

            migrationBuilder.RenameIndex(
                name: "IX_SkillLink_skillsID",
                table: "SkillLink",
                newName: "IX_SkillLink_SkillsID");

            migrationBuilder.AlterColumn<int>(
                name: "SkillsID",
                table: "SkillLink",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLink_Skills_SkillsID",
                table: "SkillLink",
                column: "SkillsID",
                principalTable: "Skills",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
