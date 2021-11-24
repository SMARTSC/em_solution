using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EM.Print.Cloud.Migrations
{
    public partial class ChangePrintOrderSchemaMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data",
                table: "printorders");

            migrationBuilder.AddColumn<string>(
                name: "permutationserialied",
                table: "printorders",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "targetcontentid",
                table: "printorders",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "permutationserialied",
                table: "printorders");

            migrationBuilder.DropColumn(
                name: "targetcontentid",
                table: "printorders");

            migrationBuilder.AddColumn<byte[]>(
                name: "data",
                table: "printorders",
                nullable: false,
                defaultValue: new byte[] {  });
        }
    }
}
