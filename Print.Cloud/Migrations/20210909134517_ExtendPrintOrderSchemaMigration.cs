using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EM.Print.Cloud.Migrations
{
    public partial class ExtendPrintOrderSchemaMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "targetcontentalbumid",
                table: "printorders",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "targetcontentalbumid",
                table: "printorders");
        }
    }
}
