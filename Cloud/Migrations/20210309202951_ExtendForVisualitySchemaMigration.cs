using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EM.Cloud.Migrations
{
    public partial class ExtendForVisualitySchemaMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_albums_images_frontimageid",
                table: "albums");

            migrationBuilder.RenameColumn(
                name: "frontimageid",
                table: "albums",
                newName: "overlayid");

            migrationBuilder.RenameIndex(
                name: "ix_albums_frontimageid",
                table: "albums",
                newName: "ix_albums_overlayid");

            migrationBuilder.AddColumn<Guid>(
                name: "overlayid",
                table: "events",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_events_overlayid",
                table: "events",
                column: "overlayid");

            migrationBuilder.AddForeignKey(
                name: "fk_albums_images_overlayid",
                table: "albums",
                column: "overlayid",
                principalSchema: "elespejomolon_sapressc_middleware_media",
                principalTable: "images",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_events_images_overlayid",
                table: "events",
                column: "overlayid",
                principalSchema: "elespejomolon_sapressc_middleware_media",
                principalTable: "images",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_albums_images_overlayid",
                table: "albums");

            migrationBuilder.DropForeignKey(
                name: "fk_events_images_overlayid",
                table: "events");

            migrationBuilder.DropIndex(
                name: "ix_events_overlayid",
                table: "events");

            migrationBuilder.DropColumn(
                name: "overlayid",
                table: "events");

            migrationBuilder.RenameColumn(
                name: "overlayid",
                table: "albums",
                newName: "frontimageid");

            migrationBuilder.RenameIndex(
                name: "ix_albums_overlayid",
                table: "albums",
                newName: "ix_albums_frontimageid");

            migrationBuilder.AddForeignKey(
                name: "fk_albums_images_frontimageid",
                table: "albums",
                column: "frontimageid",
                principalSchema: "elespejomolon_sapressc_middleware_media",
                principalTable: "images",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
