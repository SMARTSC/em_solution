using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EM.Cloud.Migrations
{
    public partial class ExtendForDataStructMigrationSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "kindid",
                table: "events",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "eventkinds",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_eventkinds", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_events_kindid",
                table: "events",
                column: "kindid");

            migrationBuilder.AddForeignKey(
                name: "fk_events_eventkinds_kindid",
                table: "events",
                column: "kindid",
                principalTable: "eventkinds",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_events_eventkinds_kindid",
                table: "events");

            migrationBuilder.DropTable(
                name: "eventkinds");

            migrationBuilder.DropIndex(
                name: "ix_events_kindid",
                table: "events");

            migrationBuilder.DropColumn(
                name: "kindid",
                table: "events");
        }
    }
}
