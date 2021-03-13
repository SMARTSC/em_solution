using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EM.Cloud.Migrations
{
    public partial class MinorImprovesMigrationSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_albums_collagelayouts_defaultphotosessioncontentcollagelayou~",
                table: "albums");

            migrationBuilder.DropForeignKey(
                name: "fk_albums_collagelayouts_defaultpicturecontentcollagelayoutid",
                table: "albums");

            migrationBuilder.DropForeignKey(
                name: "fk_albums_events_eventid",
                table: "albums");

            migrationBuilder.DropForeignKey(
                name: "fk_albums_images_overlayid",
                table: "albums");

            migrationBuilder.RenameColumn(
                name: "descrition",
                table: "events",
                newName: "description");

            migrationBuilder.AlterColumn<Guid>(
                name: "overlayid",
                table: "albums",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "eventid",
                table: "albums",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "defaultpicturecontentcollagelayoutid",
                table: "albums",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "defaultphotosessioncontentcollagelayoutid",
                table: "albums",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_albums_collagelayouts_defaultphotosessioncontentcollagelayou~",
                table: "albums",
                column: "defaultphotosessioncontentcollagelayoutid",
                principalTable: "collagelayouts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_albums_collagelayouts_defaultpicturecontentcollagelayoutid",
                table: "albums",
                column: "defaultpicturecontentcollagelayoutid",
                principalTable: "collagelayouts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_albums_events_eventid",
                table: "albums",
                column: "eventid",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_albums_images_overlayid",
                table: "albums",
                column: "overlayid",
                principalSchema: "elespejomolon_sapressc_middleware_media",
                principalTable: "images",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_albums_collagelayouts_defaultphotosessioncontentcollagelayou~",
                table: "albums");

            migrationBuilder.DropForeignKey(
                name: "fk_albums_collagelayouts_defaultpicturecontentcollagelayoutid",
                table: "albums");

            migrationBuilder.DropForeignKey(
                name: "fk_albums_events_eventid",
                table: "albums");

            migrationBuilder.DropForeignKey(
                name: "fk_albums_images_overlayid",
                table: "albums");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "events",
                newName: "descrition");

            migrationBuilder.AlterColumn<Guid>(
                name: "overlayid",
                table: "albums",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "eventid",
                table: "albums",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "defaultpicturecontentcollagelayoutid",
                table: "albums",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "defaultphotosessioncontentcollagelayoutid",
                table: "albums",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "fk_albums_collagelayouts_defaultphotosessioncontentcollagelayou~",
                table: "albums",
                column: "defaultphotosessioncontentcollagelayoutid",
                principalTable: "collagelayouts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_albums_collagelayouts_defaultpicturecontentcollagelayoutid",
                table: "albums",
                column: "defaultpicturecontentcollagelayoutid",
                principalTable: "collagelayouts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_albums_events_eventid",
                table: "albums",
                column: "eventid",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_albums_images_overlayid",
                table: "albums",
                column: "overlayid",
                principalSchema: "elespejomolon_sapressc_middleware_media",
                principalTable: "images",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
