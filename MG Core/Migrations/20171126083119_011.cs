using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MGCore.Migrations
{
    public partial class _011 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "Reply",
                type: "varchar(3000)",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 300);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "Reply",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(3000)",
                oldMaxLength: 3000);
        }
    }
}
