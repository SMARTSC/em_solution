using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EM.Print.Cloud.Migrations
{
    public partial class Extend2PrintOrderSchemaMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "targetcontenteventimageid",
                table: "printorders",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "targetcontenteventimageid",
                table: "printorders");
        }
    }
}
